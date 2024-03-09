using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using UnityEngine.XR;

public class PlayerStateMachine : MonoBehaviour
{

    #region Jump related
    private Rigidbody2D _rb;
    public float initalJumpHeight;

    [Header("Ground Check")]
    [SerializeField] Transform _groundCheck;
    [SerializeField] Vector2 _checkSize;
    public LayerMask GroundLayer;

    [Header("Fast Fall")]
    float _gravityScale;
    public float GravityMult;
    public float ClampedVelocity;

    [Header ("Timers")]
    public float bufferTime;
    float _bufferCounter;
    #endregion

    [Header ("Run")]
    [SerializeField] float acc;
    [SerializeField] float targetSpeed;

    //Inputs
    bool _jumpPressed;
    bool _jumpReleased;

    bool _isJumping;

    float _xInput;
    

    BaseState _currentState;
    StateFactory _states;

    #region Get Sets
    public float xInput {get {return _xInput;}}
    public float TargetSpeed {get {return targetSpeed;}}
    public bool JumpReleased {get {return _jumpReleased;} set{ _jumpReleased = value;}}
    public Rigidbody2D Rb {get {return _rb;}set {_rb = value;}}
    public float GravityScale {get {return _gravityScale;}}
    public bool IsJumping {get {return _isJumping;} set{_isJumping = value;}}
    public bool JumpPressed {get {return _jumpPressed;} set { _jumpPressed = value;}}
    public bool IsGounded {get {return Grounded();}}
    public float BufferCounter {get {return _bufferCounter;} set {_bufferCounter = value;}}
    public BaseState CurrentState {get {return _currentState;} set { _currentState = value;}}
    #endregion
    // Start is called before the first frame update
    void Awake()
    { 
        
        _rb = GetComponent<Rigidbody2D>();
        _gravityScale = _rb.gravityScale;
        _states = new StateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();


    }

    // Update is called once per frame
    void Update()
    {
        Inputs();

        _currentState.UpdateStates();
    }

    void Inputs(){

        _xInput = Input.GetAxisRaw("Horizontal"); 

        if(Input.GetKeyDown(KeyCode.Space)) _jumpPressed = true;

        if(_jumpPressed && !Grounded()){
            if(_bufferCounter < bufferTime){
                _bufferCounter += Time.deltaTime;
            }else{
                _jumpPressed = false;
            }
        }
        
        if(Input.GetKeyUp(KeyCode.Space) && _rb.velocity.y > 0 ) _jumpReleased = true;
    }

    public bool Grounded(){
        return Physics2D.OverlapBox(_groundCheck.position, _checkSize, 0, GroundLayer);
    }
}