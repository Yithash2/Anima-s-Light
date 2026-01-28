using System;
using System.Collections;
using System.Collections.Generic;
using Levels;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

#if UNITY_EDITOR
    using UnityEditor;
#endif
using TMPro;



public class PlayerStateMachine : MonoBehaviour
{

    public PlayerDateScriptableObject PlayerData;

    FXManager _fxManager;

    [SerializeField] ParticleSystem _speedlines;

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

    [SerializeField] Transform _ceilingCheck;
    [SerializeField] Vector2 _ceilingCheckSize;
    
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
    float _releaseBufferCounter;
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

    [SerializeField] GameObject _speedLines;

    [SerializeField] GameObject _explosion;

    public cam _camera;
    //Inputs
    bool _jumpPressed;
    bool _jumpReleased;
    bool _jumpReleasedR;

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

    private bool _wallJumpCombo = false;
    private bool _hasWallJumpCombo = false;

    [SerializeField]
    private SpriteRenderer _sprite;

    public bool CanSuffocate = true;

    public bool stuck = false;

    [SerializeField] private Transform _autostepBottomCheck;
    [SerializeField] private Transform _autostepBottomAirCheck;
    [SerializeField] private Transform _autostepTopCheck;
    [SerializeField] private Transform _autostepTopAirCheck;

    private Transform _currenTopAutostepCheck;
    private Transform _currentBottomAutostepCheck;
    
    private BoxCollider2D _boxCollider;

    public Action Dies;

    public GroundMaterial GroundType;
    
    #region Get Sets
    public FXManager FxManager{get {return _fxManager;}}
    public bool HasWallJumpCombo{get {return _hasWallJumpCombo;}}
    public bool WallJumpCombo {get {return _wallJumpCombo;} set{_wallJumpCombo = value;}}
    public int AirDashNumber{get {return _numberOfAirDash;} set{_numberOfAirDash = value;}}
    public float JumpPower {get { return _jumpPower;} set { _jumpPower = value;}}
    public float DashCounter {get{return _airDashCounter;} set {_airDashCounter = value;}} 
    public GameObject Bubble {get {return _bubble;}}
    public Animator BubbleAnimator {get {return _bubbleAnimator;}}
    public float DrowningCounter {get { return _drowningCounter; }set { _drowningCounter = value;}}
    public float DrowningTime {get { return _drowningTime;}}
    public float StoredVelocityResetCounter {get {return _platformCoyoteCounter;}set {_platformCoyoteCounter = value;}}
    public Vector2 PlatformAcceleration {get { return new Vector2(_platformAccelerationX, _platformAccelerationY);}set { _platformAccelerationX = value.x; _platformAccelerationY = value.y; } }
    public Vector2 StoredVelocity {get { return new Vector2(_latePlatformVelocityX, _latePlatformVelocityY);}set { _latePlatformVelocityX = value.x; _latePlatformVelocityY = value.y; } }
    public bool IsOnMovingPlatform {get {return _isOnMovingPlatform;}set { _isOnMovingPlatform = value;} }

    public float AccelerationTime
    {
        get { return IsTouchingGround() ? PlayerData.GroundAccelTime : PlayerData.AirAccelTime; }
    }

    public float DeccelerationTime
    {
        get { return IsTouchingGround() ? PlayerData.GroundDeccelTime : PlayerData.AirDeccelTime; }
    }
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
    public bool JumpReleased {get {return _jumpReleasedR;}}
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
    public bool IsDead {get {return _isDead;} set {_isDead = value;}}
    public bool JumppReleasedT { get { return _jumpReleased; } }

    public int NumberOfTimeStopped { get; private set; }
    [SerializeField] private float _timeStopCooldownTimer = 5;
    private float _timeStoperCooldownCounter = 0;

    Transform _playerTransform;
    public bool UpdateStates { get; private set; } = true;

    public string _deadlyTag;

    private void OnValidate()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        
        
        _currenTopAutostepCheck = _autostepTopCheck;
        _currentBottomAutostepCheck = _autostepBottomCheck;
    }
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

    public void ChangeUpdateState(bool value)
    {
        UpdateStates = value;   
    }

    void Start(){
        _rb = GetComponent<Rigidbody2D>();
        _playerTransform = GetComponent<Transform>();
        _gravityScale = _rb.gravityScale;
        _boxCollider = GetComponent<BoxCollider2D>();
        
        
        _currenTopAutostepCheck = _autostepTopCheck;
        _currentBottomAutostepCheck = _autostepBottomCheck;

        _states = new StateFactory(this);
        _currentState = _states.Normal();
        _currentState.EnterState();

        _fxManager = FXManager.Instance;

        InputsManager.Instance.Jump.started += Jump;
        InputsManager.Instance.Jump.performed += Jump;
        InputsManager.Instance.Jump.canceled += Jump;

        InputsManager.Instance.Dive.started += Dive;
        InputsManager.Instance.Dive.performed += Dive;
        InputsManager.Instance.Dive.canceled += Dive;

        InputsManager.Instance.XInput.started += Move;
        InputsManager.Instance.XInput.performed += Move;
        InputsManager.Instance.XInput.canceled += Move;

    }
    
    void Update(){
        if (IsDead) return;
        

        if(!Physics2D.OverlapBox(_groundCheck.position, _checkSize, 0, GroundLayer)){
            if(_jumpCount < 1){
                CoyoteTimer();
            }
                
        }else
        {
            ResetJump();
        }

        if (CanAutoStep() && _xInput != 0 && (_isDashing || _rb.velocity.y < 5))
        {
            AutoStep();
            _rb.velocity = new Vector2(Mathf.Sign(_xInput) * 10, _rb.velocity.y);
        }


        //if(_rb.velocity.y < 0 && _isJumping) _isJumping = false;

        Inputs();

        if(_isOnMovingPlatform && ((_platformAccelerationY != 0 && _platformVelocityY == 0)||(_platformAccelerationX != 0 && _platformVelocityX == 0))){
            _platformCoyoteCounter = 0;

        }else if(_isOnMovingPlatform){
            _platformCoyoteCounter = _platformCoyoteTime;
        }else{

        }

        if(IsSuffocating() && !_isDead)
        {
            Die();
        }

        var emission = _speedlines.emission;
        emission.enabled = Mathf.Abs(_rb.velocity.x) > 40 || Mathf.Abs(_rb.velocity.y) > 45;

        if (_timeStoperCooldownCounter < _timeStopCooldownTimer) _timeStoperCooldownCounter += Time.fixedUnscaledDeltaTime;
        else if(NumberOfTimeStopped != 0)
        {
            NumberOfTimeStopped--;
            _timeStoperCooldownCounter = 0;
        }
    }

    public void ResetJump()
    {
        _coyoteCounter = 0;
        _jumpCount = 0;
    }

    public void Die()
    {
        tag = "Respawn";
        Dies?.Invoke();
        _isDead = true;
    }

    public void HitStop(float speedRequired, float stopedTime){
        if(Mathf.Abs(_rb.velocity.magnitude) > speedRequired){
            Instantiate(_explosion, transform.position, Quaternion.identity);
            StartCoroutine(SpeedFreeze(stopedTime));
            NumberOfTimeStopped = NumberOfTimeStopped >= 5 ? 5 : NumberOfTimeStopped+1;
            _timeStoperCooldownCounter = 0;
        }
    }

    IEnumerator SpeedFreeze(float stopedTime){
        Time.timeScale = 0.55f - (0.1f * NumberOfTimeStopped);
        for (float t = 0; t <= stopedTime; t += Time.deltaTime)
        {
            AudioPlayer.Instance.MusicSpeed = Mathf.Lerp(AudioPlayer.Instance.MusicSpeed, 0.65f ,0.125f);
            yield return 0;
        }
        Time.timeScale = 1;
        AudioPlayer.Instance.MusicSpeed = 1f;
    }

    void FixedUpdate(){
        
        
        if((_isFacingRight && _xInput < 0f || !_isFacingRight && _xInput > 0f) && CanMove())
        {
            Flip();
        }

        

        StartCoroutine(LateFixedUpdate());

        if(tag == "Respawn" && IsDead){
            Respawn();
        }else if(!stuck){
            _currentState.UpdateStates();

        }
    }

    public void ForceUpdateState()
    {
        _currentState.UpdateStates();
    }
    






    void LateUpdate(){

        _lateVelocity = _rb.velocity;
        
        //_rb.velocity = Vector2.ClampMagnitude(_rb.velocity, 150f);

    }

    /*void OnCollisionEnter2D(Collision2D collision){

        if(collision.gameObject.CompareTag(_deadlyTag) && !_isDead )
        {
            Die();
        }
    }*/

    void Respawn(){
        Rb.velocity = new Vector2(0, 0);
        _bufferCounter = bufferTime;
        _jumpPressed = false;

        if (_animator.enabled)
        {
            if (!IsAnimating("Death", _animator) && !IsAnimating("Death End", _animator) && !IsAnimating("Respawn", _animator)){
                _animator.Play("Death");        
                _camera.CamAnimator.Play("Cam zoom");
                _camera.ResetPostion();
            }
        
            if(IsAnimating("Death End", _animator)){
                DeathCounter.Instance.AddToDeathCount();
                Teleport(respawnPoint);
                _animator.Play("Respawn");
                _camera.CamAnimator.Play("Cam dezoom");
                tag = "Player";
                _currentState.SwitchState(_states.Grounded());
                _isDead = false;
            }
        }
        else
        {
            if (IsAnimating("Cam normal", _camera.CamAnimator))
            {
                _camera.CamAnimator.Play("Cam zoom");
                _camera.ResetPostion();
            }

            if (IsAnimating("Cam zoom ended", _camera.CamAnimator))
            {
                DeathCounter.Instance.AddToDeathCount();
                Teleport(respawnPoint);
                _animator.Play("Respawn");
                _camera.CamAnimator.Play("Cam dezoom");
                tag = "Player";
                _currentState.SwitchState(_states.Grounded());
                _isDead = false;
            }

                
        }


    }
    
    public void Teleport(Transform target)
    {
        // Calculate the offset of the player relative to the PlayerHolder
        Vector3 playerOffset = transform.localPosition;

        // Move PlayerHolder to the target location, keeping the player's offset intact
        Vector3 targetPosition = target.position;
        transform.parent.position = targetPosition - playerOffset;
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        InputsManager.Instance.Jump.started -= Jump;
        InputsManager.Instance.Jump.performed -= Jump;
        InputsManager.Instance.Jump.canceled -= Jump;

        InputsManager.Instance.Dive.started -= Dive;
        InputsManager.Instance.Dive.performed -= Dive;
        InputsManager.Instance.Dive.canceled -= Dive;

        InputsManager.Instance.XInput.started -= Move;
        InputsManager.Instance.XInput.performed -= Move;
        InputsManager.Instance.XInput.canceled -= Move;
    }

    public bool IsAnimating(string animationName, Animator animator){
        if(animator.GetCurrentAnimatorStateInfo(0).IsName(animationName)){
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

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (!UpdateStates) {
            _bufferCounter = 0f;
            _jumpPressed = false;
            _jumpReleased = false;
            return; 
           }

        if (ctx.started)
        {
            _bufferCounter = 0;
            _jumpPressed = true;
        }

        if (ctx.canceled)
        {
            _jumpReleased = true;
        }

        _jumpReleasedR = ctx.canceled;

    }


    public void Move(InputAction.CallbackContext ctx){
        if (!UpdateStates){
            _xInput = 0;
            return; 
        }
        _xInput = ctx.ReadValue<float>();
    }

    public void Dive(InputAction.CallbackContext ctx){
        if (!UpdateStates)
        {
            Debug.LogWarning("Dive in unenabled");
            _divePressed = false;
            return;
        }

        if (ctx.performed){
            _divePressed = true;
        }else{
            _divePressed = false;
        }
    }
    
    public void StartWallJumpComboCoroutine(float time){
        StartCoroutine(WallJumpComboCoroutine(time));
    }

    IEnumerator WallJumpComboCoroutine(float time){
        _hasWallJumpCombo = true;
        yield return new WaitForSeconds(time);
        _hasWallJumpCombo = false;
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

        if (_jumpReleased)
        {
            if(_releaseBufferCounter < PlayerData.ReleaseCoyoteTimer)
            {
                _releaseBufferCounter += Time.deltaTime;
            }
            else
            {
                _releaseBufferCounter = 0;
                _jumpReleased = false;
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
        return Physics2D.OverlapCircle(_suffocationCheck.position, _suffocationRadius, GroundLayer) && CanSuffocate;
    }

    public bool IsTouchingGround(){
        Collider2D ground  = Physics2D.OverlapBox(_groundCheck.position, _checkSize, 0, GroundLayer);
        return ground;
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

    public bool IsCeilled(){
        return Physics2D.OverlapBox(_ceilingCheck.position, _ceilingCheckSize, 0, GroundLayer);
    }
    /*public bool IsWalled(){
        
        return Physics2D.OverlapBox(wallCheck.position, wallSize, 0, wallLayer);
    }*/

    public bool IsWalled()
    {
        return Physics2D.OverlapCircle(_currentBottomAutostepCheck.position, 0.1f, GroundLayer) &&
               GroundType.isWallJumpable;
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

    public bool CanAutoStep()
    {
        _currentBottomAutostepCheck = IsGrounded() || IsDashing ? _autostepBottomCheck : _autostepBottomAirCheck; 
        _currenTopAutostepCheck = IsGrounded() || IsDashing ? _autostepTopCheck : _autostepTopAirCheck;

        var ofset = Vector3.right * (transform.lossyScale.x * 0.3f);
        
        bool hasBottomObstacle = Physics2D.OverlapCircle(_currentBottomAutostepCheck.position - ofset, 0.1f, GroundLayer);
        bool hasRoomOnTop = !Physics2D.OverlapCircle(_currenTopAutostepCheck.position - ofset, 0.1f, GroundLayer);
        
        return hasBottomObstacle && hasRoomOnTop;
    }


    /*public void AutoStep()
    {
        transform.position = new Vector3(transform.position.x, _currenTopAutostepCheck.position.y, transform.position.z);
    }*/
    
    public void AutoStep()
    {
        _currentBottomAutostepCheck = IsGrounded() || IsDashing ? _autostepBottomCheck : _autostepBottomAirCheck; 
        _currenTopAutostepCheck = IsGrounded() || IsDashing ? _autostepTopCheck : _autostepTopAirCheck;
        
        Vector2 stepOrigin = _currenTopAutostepCheck.position - (Vector3.right * (transform.lossyScale.x * 0.3f));
        Vector2 stepDirection = Vector2.down;
        float maxStepHeight = _currenTopAutostepCheck.position.y - _currentBottomAutostepCheck.position.y;

        RaycastHit2D stepHit = Physics2D.Raycast(stepOrigin, stepDirection, maxStepHeight, GroundLayer);

        // Draw raycast from bottom to top checker
        Debug.DrawLine(stepOrigin, stepOrigin + stepDirection * maxStepHeight, Color.cyan, 0.5f);

        if (stepHit)
        {
            float newY = stepHit.point.y + 0.1f + _boxCollider.size.y/2 - _boxCollider.offset.y;

            // Draw hit point
            Debug.DrawRay(stepHit.point, Vector2.right * 0.2f, Color.magenta, 0.5f);
            Debug.DrawLine(transform.position, new Vector3(transform.position.x, newY, transform.position.z), Color.yellow, 0.5f);

            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
        else
        {
            // No step possible
            Debug.DrawRay(stepOrigin + stepDirection * maxStepHeight, Vector2.right * 0.2f, Color.gray, 0.5f);
        }
    }

    [HideInInspector]public float currentSpeedModifier;
    public void Run(float targetSpeed){
        currentSpeedModifier = IsTouchingGround()
            ? Mathf.Lerp(currentSpeedModifier, GroundType.maxSpeedAddifier, 0.5f * Time.deltaTime)
            : Mathf.Lerp(currentSpeedModifier, 1f, 0.5f * Time.deltaTime);

        Vector2 vel = new Vector2(targetSpeed, Rb.velocity.y);
        if (Mathf.Abs(Rb.velocity.x) > targetSpeed)
        {
            Rb.velocity = new Vector2(Mathf.Lerp(Rb.velocity.x, vel.x, 0.5f * Time.deltaTime), Rb.velocity.y);
        }
        else
        {
            Rb.velocity = new Vector2(vel.x, Rb.velocity.y);
        }
        
    }

    public void Acceleration(float targetSpeed)
    {
        currentSpeedModifier = IsTouchingGround()
            ? Mathf.Lerp(currentSpeedModifier, GroundType.maxSpeedAddifier, 0.5f * Time.deltaTime)
            : Mathf.Lerp(currentSpeedModifier, 1f, 0.5f * Time.deltaTime);

        float rate =
            Mathf.Abs(targetSpeed) / AccelerationTime *
            (IsTouchingGround() ? GroundType.adhesion : 1f);

        float newX = Mathf.MoveTowards(
            Rb.velocity.x,
            targetSpeed,
            rate * Time.deltaTime
        );

        Rb.velocity = new Vector2(newX, Rb.velocity.y);
    }


    private float friction;
    public void Deceleration(float factor = 1f){
        friction = IsTouchingGround() ? GroundType.friction : Mathf.Lerp(friction, 1, 0.5f * Time.deltaTime);
        
        float rate =
            Mathf.Abs(Rb.velocity.x)/DeccelerationTime * friction ;

        float newX = Mathf.MoveTowards(
            Rb.velocity.x,
            0,
            rate * Time.deltaTime * factor
        );

        Rb.velocity = new Vector2(newX, Rb.velocity.y);
   
    }







    void OnDrawGizmos(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_suffocationCheck.position,_suffocationRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_groundCheck.position, _checkSize);
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(spaceCheck.position, spaceSize);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(_ceilingCheck.position, _ceilingCheckSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_currentBottomAutostepCheck.position, 0.1f);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_currenTopAutostepCheck.position, 0.1f);
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_currentBottomAutostepCheck.position - (Vector3.right * transform.lossyScale.x * 0.3f), 0.1f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(_currenTopAutostepCheck.position - (Vector3.right * transform.lossyScale.x * 0.3f), 0.1f);




#if UNITY_EDITOR
        if (_currentState != null)
            Handles.Label(transform.position + new Vector3(-2, 4, 0), _currentState.CurrentStates());
        #endif
    }

}
