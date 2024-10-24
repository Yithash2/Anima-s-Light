using System.Collections;
using System.Globalization;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
#if UNITY_EDITOR
    using UnityEditor;
#endif
using TMPro;



public class PlayerStateMachine : MonoBehaviour
{

    public PlayerDateScriptableObject PlayerData;

    [SerializeField] Animator _animator;
    [SerializeField] AnimationClip[] _animations;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip[] _audioClips;

    #region Jump related
    private Rigidbody2D _rb;
    private Vector2 _lateVelocity;
    private Vector2 _acceleration;
    private float _platformVelocityY;
    [SerializeField] private int _numberOfJumps = 1;
    private int _jumpCount;
    private float _jumpPower;

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

    [SerializeField] Transform _suffocationCheck;
    [SerializeField] float _suffocationRadius;

    [Header("Fast Fall")]
    float _gravityScale;

    [Header ("Timers")]
    [SerializeField] float bufferTime;
    float _bufferCounter;
    float _wallJumpCounter = 10000;
    [SerializeField] float _coyoteTime;

    [SerializeField] float _drowningTime = 10;
    float _drowningCounter;
    float _coyoteCounter;

    [SerializeField] float _airDashTime;
    float _airDashCounter;

    int _numberOfAirDash;

    #endregion

    [SerializeField] private float _platformVelocityX;
    [SerializeField] private float _platformCoyoteTime;
    [ReadOnly, SerializeField] private float _platformCoyoteCounter;
    private float _accelerationTime;
    [SerializeField, Range(0.01f, 1)] private float _airDrag;

    public cam _camera;
    //Inputs
    bool _jumpPressed;
    bool _jumpReleased;

    bool _isJumping;
    bool _isDashing;
    bool _isWallJumping;

    float _xInput;
    bool _divePressed;
    bool _isFacingRight;

    public Transform respawnPoint;
    
    BaseState _currentState;
    StateFactory _states;

    private bool _isOnMovingPlatform;
    private float _latePlatformVelocityX;
    private float _latePlatformVelocityY;
    private float _platformAccelerationX;
    private float _platformAccelerationY;

    private bool _isDead;

    [SerializeField] TextMeshPro StateInfo;

    [SerializeField] GameObject _bubble;
    [SerializeField] Animator _bubbleAnimator;

    #region Get Sets
    public int AirDashNumber{get {return _numberOfAirDash;} set{_numberOfAirDash = value;}}
    public float JumpPower {get { return _jumpPower;} set { _jumpPower = value;}}
    public Vector2 Acceleration  {get {return _acceleration;}}
    public float DashCounter {get{return _airDashCounter;} set {_airDashCounter = value;}} 
    public GameObject Bubble {get {return _bubble;}}
    public Animator BubbleAnimator {get {return _bubbleAnimator;}}
    public float DrowningCounter {get { return _drowningCounter; }set { _drowningCounter = value;}}
    public float DrowningTime {get { return _drowningTime;}}
    public float StoredVelocityResetCounter {get {return _platformCoyoteCounter;}set {_platformCoyoteCounter = value;}}
    public Vector2 PlatformAcceleration {get { return new Vector2(_platformAccelerationX, _platformAccelerationY);}set { _platformAccelerationX = value.x; _platformAccelerationY = value.y; } }
    public Vector2 StoredVelocity {get { return new Vector2(_latePlatformVelocityX, _latePlatformVelocityY);}set { _latePlatformVelocityX = value.x; _latePlatformVelocityY = value.y; } }
    public bool IsOnMovingPlatform {get {return _isOnMovingPlatform;}set { _isOnMovingPlatform = value;} }
    public float AccelerationTime {get { return _accelerationTime;} set { _accelerationTime = value; } }
    public float PlatformVelocityX {get {return _platformVelocityX;} set{_platformVelocityX = value;}}
    public float PlatformVelocityY {get {return _platformVelocityY;} set{_platformVelocityY = value;}}
    public int NumberOfJumps {get {return _numberOfJumps;} set{_numberOfJumps = value;}}
    public int JumpCount {get {return _jumpCount;} set{_jumpCount = value;}}
    public AudioSource AudioSource {get {return _audioSource;}}
    public AudioClip[] AudioClips {get {return _audioClips;}}
    public float WallJumpCounter {get {return _wallJumpCounter;}set {_wallJumpCounter = value;}}
    public bool DivePressed {get {return _divePressed;}}
    public Animator Animator {get {return _animator;}}
    public AnimationClip[] Animations {get {return _animations;}}
    public float XInput {get {return _xInput;}}
    public bool JumpReleased {get {return _jumpReleased;} set{ _jumpReleased = value;}}
    public Rigidbody2D Rb {get {return _rb;}set {_rb = value;}}
    public float GravityScale {get {return _gravityScale;}}
    public bool IsJumping {get {return _isJumping;} set{_isJumping = value;}}
    public bool IsDashing {get {return _isDashing;} set{_isDashing = value;}}
    public bool IsWallJumping {get {return _isWallJumping;} set{_isWallJumping = value;}}
    public bool JumpPressed {get {return _jumpPressed;} set { _jumpPressed = value;}}
    public float BufferCounter {get {return _bufferCounter;} set {_bufferCounter = value;}}
    public float CoyoteTime {get {return _coyoteTime;}}
    public float CoyoteCounter {get {return _coyoteCounter;} set {_coyoteCounter = value;}}
    public BaseState CurrentState {get {return _currentState;} set { _currentState = value;}}
    public Transform PlayerTransform {get {return _playerTransform;}}
    public float YCamOffcet {get {return _camera.verticalShift;} set{_camera.verticalShift = value;}}
    public bool IsDead {get {return _isDead;} set {_isDead = value;}}

    Transform _playerTransform;

    public string _deadlyTag;
    #endregion

    public static PlayerStateMachine Instance {get; private set;}
    // Start is called before the first frame update
    void Awake(){
        if(Instance != null){
            Debug.Log("Found more then one PlayerStateMachine, and destoyed it");
            //Destroy(this.gameObject);
            return;
        }
        Instance = this;

    }

    void Start(){
        _rb = GetComponent<Rigidbody2D>();
        _playerTransform = GetComponent<Transform>();
        _gravityScale = _rb.gravityScale;

        _states = new StateFactory(this);
        _currentState = _states.Normal();
        _currentState.EnterState();

        
    }
    
    void Update(){
        if(!Physics2D.OverlapBox(_groundCheck.position, _checkSize, 0, GroundLayer)){
            if(_jumpCount < 1){
                CoyoteTimer();
            }
                
        }else{
            _coyoteCounter = 0;
            _jumpCount = 0;
        }

        //if(_rb.velocity.y < 0 && _isJumping) _isJumping = false;

        Inputs();

        if(_isOnMovingPlatform && ((_platformAccelerationY != 0 && _platformVelocityY == 0)||(_platformAccelerationX != 0 && _platformVelocityX == 0))){
            _platformCoyoteCounter = 0;

        }else if(_isOnMovingPlatform){
            _platformCoyoteCounter = _platformCoyoteTime;
        }else{

        }

        if(IsSuffocating() && !_isDead){
            tag = "Respawn";
            _isDead = true;
        }
    

    }

    void FixedUpdate(){
        
        if((_isFacingRight && _xInput < 0f || !_isFacingRight && _xInput > 0f) && CanMove())
        {
            Flip();
        }

        StartCoroutine(LateFixedUpdate());

        if(tag == "Respawn" && IsDead){
            Respawn();
        }else{
            _currentState.UpdateStates();
        }
 

    }

    void LateUpdate(){

        _lateVelocity = _rb.velocity;
        
        _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, 150f);

    }

    void OnCollisionEnter2D(Collision2D collision){

        if(collision.gameObject.CompareTag(_deadlyTag) && !_isDead){
            tag = "Respawn";
            _isDead = true;
        }
    }

    void Respawn(){
        Rb.velocity = new Vector2(0, 0);
        if(!IsAnimating("Death") && !IsAnimating("Death End") && !IsAnimating("Respawn")){
            _animator.Play("Death");
            _camera.CamAnimator.Play("Cam zoom");
            _camera.verticalShift = 0;
            
        }
        
        if(IsAnimating("Death End")){
            DeathCounter.Instance.AddToDeathCount();
            transform.position = respawnPoint.position;
            _animator.Play("Respawn");
            _camera.CamAnimator.Play("Cam dezoom");
            tag = "Player";
            _isDead = false;
        }
        
        

    }

    public bool IsAnimating(string animationName){
        if(_animator.GetCurrentAnimatorStateInfo(0).IsName(animationName)){
            return true;
        }else{
            return false;
        }
    }

    void PlatformCoyoteTimer(){
        if(_platformCoyoteCounter < _platformCoyoteTime){
            _platformCoyoteCounter+= Time.deltaTime;

        }else{
            if(!(_platformAccelerationY != 0 && _platformVelocityY == 0) && !(_platformAccelerationX != 0 && _platformVelocityX == 0)){
                _latePlatformVelocityX = _platformVelocityX;
                _latePlatformVelocityY = _platformVelocityY;
            }
            

        }
    }

    IEnumerator LateFixedUpdate(){
        yield return new WaitForEndOfFrame();

        _acceleration = new Vector2(_lateVelocity.x - _rb.velocity.x, _lateVelocity.y - _rb.velocity.y);



        if(_isOnMovingPlatform){
            _platformAccelerationX = (_latePlatformVelocityX - _platformVelocityX)/Time.fixedDeltaTime;
            _platformAccelerationY = (_latePlatformVelocityY - _platformVelocityY)/Time.fixedDeltaTime;      
         
        }else if(!_isJumping){    
            _latePlatformVelocityX = Mathf.Lerp(_latePlatformVelocityX, 0, _airDrag);
            _latePlatformVelocityY = Mathf.Lerp(_latePlatformVelocityY, 0, _airDrag);
            _platformVelocityX = Mathf.Lerp(_platformVelocityX, 0, _airDrag);
            _platformVelocityY = Mathf.Lerp(_platformVelocityY, 0, _airDrag);
        }

        PlatformCoyoteTimer();
        
        
        if(_isJumping && ((_platformAccelerationY != 0 && _platformVelocityY == 0)||(_platformAccelerationX != 0 && _platformVelocityX == 0))){
            _rb.velocity += _latePlatformVelocityY < 0? new Vector2(_latePlatformVelocityX + (_playerTransform.localScale.x * _latePlatformVelocityY),0) : new Vector2(_latePlatformVelocityX,0); 
            yield return 5;
            yield return new WaitForFixedUpdate();
            yield return new WaitForEndOfFrame();
            //_isJumping = false;
        }
        
    }

    public void Jump(InputAction.CallbackContext ctx){
        if(ctx.started){
            _bufferCounter = 0;
            _jumpPressed = true;
        }
        
        if(!ctx.performed){
            _jumpReleased = true;
        }
    }

    public void Move(InputAction.CallbackContext ctx){
        _xInput = ctx.ReadValue<float>();
    }

    public void Dive(InputAction.CallbackContext ctx){
        if(ctx.performed){
            _divePressed = true;
        }else{
            _divePressed = false;
        }
    }
    
    void Inputs(){

        

        if(_jumpPressed && !IsGrounded() && !IsWalled()){
            if(_bufferCounter < bufferTime){
                _bufferCounter += Time.deltaTime;
            }else{
                _bufferCounter = 0;
                _jumpPressed = false;
            }
        }

    }

    void CoyoteTimer(){
        float _dynamicCoyoteTime = (Mathf.Abs(_rb.velocity.x) > 20)? (Mathf.Sqrt(Mathf.Sign(_rb.velocity.x)*_rb.velocity.x -20)/10+CoyoteTime) : CoyoteTime;
        if(_coyoteCounter < _dynamicCoyoteTime){
            _coyoteCounter += Time.deltaTime;
            _jumpCount = 0;
        }else{
           _jumpCount = 1;
        }

    }

    public bool CanJump(){

        if(_jumpCount < _numberOfJumps){
            return true;
        }else{
            return false;
        }

    }

    public bool IsSuffocating(){
        return Physics2D.OverlapCircle(_suffocationCheck.position, _suffocationRadius, GroundLayer);
    }

    public bool IsTouchingGround(){
        return Physics2D.OverlapBox(_groundCheck.position, _checkSize, 0, GroundLayer);
    }

    public bool IsGrounded(){
        if(_coyoteCounter <= _coyoteTime){
            try{
                if(!Physics2D.OverlapBox(_groundCheck.position, _checkSize, 0, GroundLayer).CompareTag("MovingPlatform")){
                    _platformVelocityX = 0;
                    _platformVelocityY = 0;
                }
            }catch{

            }
            return true;
        }else{
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
 
        if(!_currentState.CanTurn(_currentState)){
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

    public bool CanDashInAir(){
        return _numberOfAirDash < 1;
    }

    public void UpdateDashTimer(){
        if(_airDashCounter < _airDashTime){
            _airDashCounter += Time.deltaTime;
        }else{
            _numberOfAirDash = 1;
        }

        //Debug.Log(_airDashCounter.ToString() + ","+ _numberOfAirDash.ToString());
    }
    void OnDrawGizmos(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_suffocationCheck.position,_suffocationRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(wallCheck.position, wallSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_groundCheck.position, _checkSize);
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(spaceCheck.position, spaceSize);

        #if UNITY_EDITOR
            if(_currentState != null)
            Handles.Label(transform.position + new Vector3(-2, 4, 0), _currentState.CurrentStates());
        #endif
    }

}
