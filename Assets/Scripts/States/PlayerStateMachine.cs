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
    [SerializeField] private float _baselineVelocityY;
    [SerializeField] private int _numberOfJumps = 1;
    private int _jumpCount;

    [Header("Check")]
    [SerializeField] Transform _groundCheck;
    [SerializeField] Vector2 _checkSize;
    [SerializeField] LayerMask GroundLayer;
    
    [SerializeField] Transform wallCheck;
    [SerializeField] Vector2 wallSize;
    [SerializeField] LayerMask wallLayer;

    [SerializeField] Transform spaceCheck;
    [SerializeField] Vector2 spaceSize;
    [SerializeField] LayerMask SpaceLayer;

    [Header("Fast Fall")]
    float _gravityScale;

    [Header ("Timers")]
    [SerializeField] float bufferTime;
    float _bufferCounter;
    float _wallJumpCounter;

    [SerializeField] float _coyoteTime;
    float _coyoteCounter;

    #endregion

    [SerializeField] private float _baselineVelocityX;
    private float _acceleration;

    public cam _camera;
    //Inputs
    bool _jumpPressed;
    bool _jumpReleased;

    bool _isJumping;

    float _xInput;
    float _yInput;
    bool _isFacingRight;

    public Transform respawnPoint;
    
    BaseState _currentState;
    StateFactory _states;



    #region Get Sets
    public float Acceleration {get { return _acceleration;} set { _acceleration = value; } }
    public float BaselineVelocityX {get {return _baselineVelocityX;} set{_baselineVelocityX = value;}}
    public float BaselineVelocityY {get {return _baselineVelocityY;} set{_baselineVelocityY = value;}}
    public int NumberOfJumps {get {return _numberOfJumps;} set{_numberOfJumps = value;}}
    public int JumpCount {get {return _jumpCount;} set{_jumpCount = value;}}
    public AudioSource AudioSource {get {return _audioSource;}}
    public AudioClip[] AudioClips {get {return _audioClips;}}
    public float WallJumpCounter {get {return _wallJumpCounter;}set {_wallJumpCounter = value;}}
    public float YInput {get {return _yInput;}}
    public Animator Animator {get {return _animator;}}
    public AnimationClip[] Animations {get {return _animations;}}
    public float XInput {get {return _xInput;}}
    public bool JumpReleased {get {return _jumpReleased;} set{ _jumpReleased = value;}}
    public Rigidbody2D Rb {get {return _rb;}set {_rb = value;}}
    public float GravityScale {get {return _gravityScale;}}
    public bool IsJumping {get {return _isJumping;} set{_isJumping = value;}}
    public bool JumpPressed {get {return _jumpPressed;} set { _jumpPressed = value;}}
    public float BufferCounter {get {return _bufferCounter;} set {_bufferCounter = value;}}
    public float CoyoteTime {get {return _coyoteTime;}}
    public float CoyoteCounter {get {return _coyoteCounter;} set {_coyoteCounter = value;}}
    public BaseState CurrentState {get {return _currentState;} set { _currentState = value;}}
    public Transform PlayerTransform {get {return _playerTransform;}}
    public float SmoothSpeed {get {return _camera.smooth_speed;} set{_camera.smooth_speed = value;}}
    public float YCamOffcet {get {return _camera.verticalShift;} set{_camera.verticalShift = value;}}

    Transform _playerTransform;

    public string _deadlyTag;
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
        if(!Physics2D.OverlapBox(_groundCheck.position, _checkSize, 0, GroundLayer)){
            CoyoteTimer();    
        }else{
            _coyoteCounter = 0;
            _jumpCount = 0;
        }

        Inputs();

    }

    

    void OnCollisionEnter2D(Collision2D collision){

        if(collision.gameObject.CompareTag(_deadlyTag)){
            Rb.velocity = new Vector2(0, 0);
            transform.position = respawnPoint.position;
        }
    }

    void FixedUpdate(){

        _currentState.UpdateStates();

        if((_isFacingRight && _xInput < 0f || !_isFacingRight && _xInput > 0f) && CanMove())
        {
            Flip();
        }

        



        

    }

    void Inputs(){
        _yInput = Input.GetAxisRaw("Vertical");

        _xInput = Input.GetAxisRaw("Horizontal"); 

        if(Input.GetKeyDown(KeyCode.Space)) _jumpPressed = true;

        if((_jumpPressed && !IsGrounded()) ^ (_jumpPressed && !IsWalled())){
            if(_bufferCounter < bufferTime){
                _bufferCounter += Time.deltaTime;
                Debug.Log(_bufferCounter);
            }else{
                _jumpPressed = false;
                _bufferCounter = 0;
            }
        }
        
        if(Input.GetKeyUp(KeyCode.Space)) _jumpReleased = true; else _jumpReleased = false;
    }

    void CoyoteTimer(){
        if(_coyoteCounter < CoyoteTime){
            _coyoteCounter += Time.deltaTime;
            _jumpCount = 0;
        }

    }

    public bool CanJump(){

        if(_jumpCount < _numberOfJumps){
            return true;
        }else{
            return false;
        }

    }

    public bool IsGrounded(){
        if(_coyoteCounter <= _coyoteTime){
            try{
                if(!Physics2D.OverlapBox(_groundCheck.position, _checkSize, 0, GroundLayer).CompareTag("MovingPlatform")){
                    _baselineVelocityX = 0;
                    _baselineVelocityY = 0;
                }
            }catch{

            }
            return true;
        }else{
            return false;
        }
    }

    public bool IsOnMovingPlatform(){
        try{
            if((!Physics2D.OverlapBox(_groundCheck.position, _checkSize, 0, GroundLayer).CompareTag("MovingPlatform")) || (!Physics2D.OverlapBox(spaceCheck.position, spaceSize, 0, SpaceLayer).CompareTag("MovingPlatform")) || (!Physics2D.OverlapBox(wallCheck.position, wallSize, 0, wallLayer).CompareTag("MovingPlatform"))){
                _baselineVelocityX = 0;
                _baselineVelocityY = 0;
                return false;
            }{
                return true;
            }

        }catch{
                return false;
        }
    }

    public bool IsSpaced(){
        return Physics2D.OverlapBox(spaceCheck.position, spaceSize, 0, SpaceLayer);
    }

    public bool IsWalled(){
        
        return Physics2D.OverlapBox(wallCheck.position, wallSize, 0, wallLayer);
    }

    public bool CanMove(){

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
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(spaceCheck.position, spaceSize);
    }

}
