using System.Globalization;
using UnityEngine;


public class PlayerStateMachine : MonoBehaviour
{

    public PlayerDateScriptableObject PlayerData;

    [SerializeField] Animator _animator;
    [SerializeField] AnimationClip[] _animations;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip[] _audioClips;

    #region Jump related
    private Rigidbody2D _rb;

    [Header("Check")]
    [SerializeField] Transform _groundCheck;
    [SerializeField] Vector2 _checkSize;
    [SerializeField] LayerMask GroundLayer;
    
    [SerializeField] Transform wallCheck;
    [SerializeField] Vector2 wallSize;
    [SerializeField] LayerMask wallLayer;

    [Header("Fast Fall")]
    float _gravityScale;

    [Header ("Timers")]
    [SerializeField] float bufferTime;
    float _bufferCounter;
    float _wallJumpCounter;

    #endregion

    [Header ("Run")]
    float _runAccelAmount;
    float _runDeccelAmount;
    float _maxVelocity;
    
    float _accelRate;
    float _targetSpeed;

    public cam _camera;
    //Inputs
    bool _jumpPressed;
    bool _jumpReleased;

    bool _isJumping;

    float _xInput;
    float _yInput;
    bool _isFacingRight;
    

    BaseState _currentState;
    StateFactory _states;

    #region Get Sets
    public float RunAccelAmount {get {return _runAccelAmount;}}
    public float RunDeccelAmount {get {return _runDeccelAmount;}}
    public float MaxVelocity {get {return _maxVelocity;} set {_maxVelocity = value;}}
    public AudioSource AudioSource {get {return _audioSource;}}
    public AudioClip[] AudioClips {get {return _audioClips;}}
    public float WallJumpCounter {get {return _wallJumpCounter;}set {_wallJumpCounter = value;}}
    public float YInput {get {return _yInput;}}
    public float TargetSpeed {get{return _targetSpeed;} set {_targetSpeed = value;}}
    public float AccelRate {get{return _accelRate;} set{_accelRate = value;}}
    public Animator Animator {get {return _animator;}}
    public AnimationClip[] Animations {get {return _animations;}}
    public float XInput {get {return _xInput;}}
    public bool JumpReleased {get {return _jumpReleased;} set{ _jumpReleased = value;}}
    public Rigidbody2D Rb {get {return _rb;}set {_rb = value;}}
    public float GravityScale {get {return _gravityScale;}}
    public bool IsJumping {get {return _isJumping;} set{_isJumping = value;}}
    public bool JumpPressed {get {return _jumpPressed;} set { _jumpPressed = value;}}
    public bool IsGounded {get {return Grounded();}}
    public bool IsWalled{get {return Walled();}}
    public float BufferCounter {get {return _bufferCounter;} set {_bufferCounter = value;}}
    public BaseState CurrentState {get {return _currentState;} set { _currentState = value;}}
    public Transform PlayerTransform {get {return _playerTransform;}}
    public float SmoothSpeed {get {return _camera.smooth_speed;} set{_camera.smooth_speed = value;}}
    public float YCamOffcet {get {return _camera.verticalShift;} set{_camera.verticalShift = value;}}
    public bool CanMove {get {return _canMove();}}

    Transform _playerTransform;
    #endregion
    // Start is called before the first frame update
    void Awake()
    { 
        
        _rb = GetComponent<Rigidbody2D>();
        _playerTransform = GetComponent<Transform>();
        _gravityScale = _rb.gravityScale;

        _states = new StateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();


    }

    // Update is called once per frame
    void Update()
    {

        Inputs();

        _runAccelAmount = 60 * PlayerData.RunAcceleration / _maxVelocity;
        _runDeccelAmount = 60 * PlayerData.RunDecceleration / _maxVelocity;

        
    }

    void FixedUpdate(){

        _currentState.UpdateStates();

        if((_isFacingRight && _xInput < 0f || !_isFacingRight && _xInput > 0f) && _canMove())
        {
            Flip();
        }

    }

    void Inputs(){
        _yInput = Input.GetAxisRaw("Vertical");

        _xInput = Input.GetAxisRaw("Horizontal"); 

        if(Input.GetKeyDown(KeyCode.Space)) _jumpPressed = true;

        if((_jumpPressed && !Grounded()) ^ (_jumpPressed && !Walled())){
            if(_bufferCounter < bufferTime){
                _bufferCounter += Time.deltaTime;
            }else{
                _jumpPressed = false;
            }
        }
        
        if(Input.GetKeyUp(KeyCode.Space)) _jumpReleased = true; else _jumpReleased = false;
    }

    bool Grounded(){
        return Physics2D.OverlapBox(_groundCheck.position, _checkSize, 0, GroundLayer);
    }

    bool Walled(){
        return Physics2D.OverlapBox(wallCheck.position, wallSize, 0, wallLayer);
    }

    bool _canMove(){

        if(_wallJumpCounter < PlayerData.WallJumpControlTime){
            return false;
        }else{
            return true;
        }
    }

    public void Flip(){
        _isFacingRight = !_isFacingRight;
        Vector3 theScale = _playerTransform.localScale;
        theScale.x *= -1f;
        _playerTransform.localScale = theScale;
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(wallCheck.position, wallSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_groundCheck.position, _checkSize);
    }

}