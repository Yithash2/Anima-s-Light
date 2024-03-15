using System.Globalization;
using UnityEngine;


public class PlayerStateMachine : MonoBehaviour
{

    [SerializeField] Animator _animator;
    [SerializeField] AnimationClip[] _animations;

    #region Jump related
    private Rigidbody2D _rb;
    [SerializeField] float initalJumpHeight;
    [SerializeField] float wallJumpHeight;
    [SerializeField] float wallJumpLength;

    [Header("Check")]
    [SerializeField] Transform _groundCheck;
    [SerializeField] Vector2 _checkSize;
    [SerializeField] LayerMask GroundLayer;
    
    [SerializeField] Transform wallCheck;
    [SerializeField] Vector2 wallSize;
    [SerializeField] LayerMask wallLayer;

    [Header("Fast Fall")]
    float _gravityScale;
    [SerializeField] float gravityMult;
    [SerializeField] float groundSlamMult;
    [SerializeField] float clampedVelocity;
    [SerializeField] float slideSpeed;

    [Header ("Timers")]
    [SerializeField] float bufferTime;
    float _bufferCounter;
    #endregion

    [Header ("Run")]
    [SerializeField] float acc;

    [SerializeField] float _timeToAccelerate;
    [SerializeField] float _maxVelocity;
    [SerializeField] float _runAcceleration;
    [SerializeField] float _runDecceleration;
    [SerializeField] float _airMult;
    float _accelRate;
    float _targetSpeed;
    float _runAccelAmount;
    float _runDeccelAmount;


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
    public float YInput {get {return _yInput;}}
    public float GroundSlamMult {get {return groundSlamMult;}}
    public float MaxVelocity {get {return _maxVelocity;}}
    public float AirMult {get {return _airMult;}}
    public float RunAccelAmount {get {return _runAccelAmount;}}
    public float RunDeccelAmount {get {return _runDeccelAmount;}}
    public float TargetSpeed {get{return _targetSpeed;} set {_targetSpeed = value;}}
    public float AccelRate {get{return _accelRate;} set{_accelRate = value;}}
    public Animator Animator {get {return _animator;}}
    public AnimationClip[] Animations {get {return _animations;}}
    public float SlideSpeed {get {return slideSpeed;}}
    public float Accel {get {return acc;}}
    public float XInput {get {return _xInput;}}
    public float ClampedVelocity {get {return clampedVelocity;}}
    public float GravityMult {get {return gravityMult;}}
    public float InitalJumpHeight {get {return initalJumpHeight;}}
    public float WallJumpHeight {get {return wallJumpHeight;}}
    public float WallJumpLength {get {return wallJumpLength;}}
    public bool JumpReleased {get {return _jumpReleased;} set{ _jumpReleased = value;}}
    public Rigidbody2D Rb {get {return _rb;}set {_rb = value;}}
    public float GravityScale {get {return _gravityScale;}}
    public bool IsJumping {get {return _isJumping;} set{_isJumping = value;}}
    public bool JumpPressed {get {return _jumpPressed;} set { _jumpPressed = value;}}
    public bool IsGounded {get {return Grounded();}}
    public bool isWalled{get {return Walled();}}
    public float BufferCounter {get {return _bufferCounter;} set {_bufferCounter = value;}}
    public BaseState CurrentState {get {return _currentState;} set { _currentState = value;}}
    public Transform PlayerTransform {get {return _playerTransform;}}
    public float SmoothSpeed {get {return _camera.smooth_speed;} set{_camera.smooth_speed = value;}}

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

    void Start(){
        
    }

    // Update is called once per frame
    void Update()
    {

        _runAccelAmount = 60 * _runAcceleration / _maxVelocity;
        _runDeccelAmount = 60 * _runDecceleration / _maxVelocity;

        Inputs();
        
    }

    void FixedUpdate(){

        _currentState.UpdateStates();

        if(_isFacingRight && _xInput < 0f || !_isFacingRight && _xInput > 0f)
        {
            Flip();
        }

    }

    void Inputs(){
        _yInput = Input.GetAxisRaw("Vertical");

        _xInput = Input.GetAxisRaw("Horizontal"); 

        if(Input.GetKeyDown(KeyCode.Space)) _jumpPressed = true;

        if(_jumpPressed && !Grounded()){
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