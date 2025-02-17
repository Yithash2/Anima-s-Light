using UnityEngine;
using System.Collections;
public class DashState : BaseState{
    
    public DashState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        _stateTimer = 0.613f;
        _canTurn = false;
    }

    bool _hasCeilled;

    float horizontalJumpForce;
    public override void EnterState()
    {   
        
        
        _ctx.HitStop(50f, 0.3f);
        

        if(!_ctx.IsAnimating("Respawn")) _ctx.Animator.Play(_ctx.Animations[2].name);
        _ctx.AudioSource.clip = _ctx.AudioClips[0]; 

        _ctx.Rb.gravityScale = _ctx.GravityScale * _ctx.PlayerData.GravityMult;

        _stateTimeCounter = 0f;

        _canTurn = false;

        _ctx.AudioSource.Play();

        _ctx.Rb.velocity = new Vector2(Mathf.Lerp(_ctx.Rb.velocity.x, Mathf.Sign(_ctx.Rb.velocity.x)*horizontalJumpForce, 0.25f), _ctx.Rb.velocity.y);

        if(_hasCeilled){
            _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x* 1.5f, -_ctx.Rb.velocity.y * 1.5f);
            _hasCeilled = false;
            _ctx.Rb.velocity = new Vector2(Mathf.Clamp(_ctx.Rb.velocity.x, -150, 150),_ctx.Rb.velocity.y);   
        }

        HandleJumpStart();
        
    }

    public override void UpdateState(){      

        _stateTimeCounter += Time.fixedDeltaTime;
        
        if(_stateTimeCounter >= _stateTimer/2){
            if(-Mathf.Sign(_ctx.XInput) == Mathf.Sign(_ctx.Rb.velocity.x) 
            && _ctx.XInput != 0 
            && _ctx.Rb.velocity.x!=0){
                _ctx.Rb.AddForce(new Vector2(5*_ctx.XInput,0));
            }
            

            if(_stateTimeCounter >= _stateTimer*3/4) _canTurn = true;
        }

        if(_ctx.IsCeilled() && !_hasCeilled && _ctx.JumpPressed){
            _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x* 1.5f, -_ctx.Rb.velocity.y* 1.5f); 
            _ctx.HitStop(0f, 0.2f);
            _hasCeilled = true;
        }

        _ctx.YCamOffcet = Mathf.Lerp(_ctx.YCamOffcet, 5, 0.125f);
        
        if(Mathf.Abs(_ctx.Rb.velocity.x) > 80){
            _ctx.Rb.velocity = new Vector2(Mathf.Lerp(_ctx.Rb.velocity.x, Mathf.Sign(_ctx.Rb.velocity.x)*80, 0.125f), _ctx.Rb.velocity.y);  
        }

        if(_ctx.IsTouchingGround()){
            _stateTimeCounter = 100f;
        }

        CheckSwitchState();

        
        
    }

    public override void QuitState()
    {
        
        _stateTimeCounter = 100f;
        _canTurn = true;

        _hasCeilled = false;

        _ctx.IsDashing = false;

    }

    public override void CheckSwitchState()
    {
        if(_ctx.Rb.velocity.x < horizontalJumpForce/3 && _ctx.IsSpaced()){
            _stateTimeCounter = 100f;
        }else if(_ctx.IsWalled()){
            if(!_ctx.JumpPressed){
                _ctx.Rb.velocity = new Vector2(0,horizontalJumpForce/5 );
                _ctx.DashCounter = 0;
                _ctx.StartWallJumpComboCoroutine(0.5f);
                _ctx.WallJumpCombo = true;
                SwitchState(_factory.WallJump());
            }

            _stateTimeCounter = 100f;
        }else if(_ctx.DivePressed && _ctx.JumpPressed && _ctx.IsGrounded() && _ctx.XInput != 0){ 
                SwitchState(_factory.Dash());
        }else if(_ctx.DivePressed && _stateTimeCounter >= _stateTimer *95/100){
            SwitchState(_factory.Dive());
        }else if(_stateTimeCounter >= _stateTimer){
            if(_ctx.IsTouchingGround()){
                if(_ctx.JumpPressed && _ctx.CanJump()){
                    SwitchState(_factory.JumpStart());
                }else if(_ctx.XInput != 0){
                    SwitchState(_factory.Running());
                }else{
                    SwitchState(_factory.Iddle()); 
                }
            }else{
                if(_ctx.XInput != 0){
                    SwitchState(_factory.Controlling());
                }else{
                    SwitchState(_factory.Fall()); 
                }
            }
        }  
    }

    public override void InitialiseSubState()
    {

    }

    void HandleJumpStart(){

        if(_ctx.StoredVelocity.y < 0){
            _ctx.StoredVelocity = Vector2.zero;
        }

        _ctx.JumpPressed = false;

        _ctx.IsDashing = true;

        _ctx.DashCounter = 100000;

        _ctx.CoyoteCounter = _ctx.CoyoteTime + 1;
        
        _ctx.BufferCounter = 0;
       
        float gravity = Physics2D.gravity.y * _ctx.Rb.gravityScale;

        float verticalJumpForce = Mathf.Sqrt((3 + _ctx.StoredVelocity.y) * -2 * gravity);
        float timeToPeak = verticalJumpForce / Mathf.Abs(gravity);
        float totalTimeInAir = 2 * timeToPeak;
        horizontalJumpForce = ((20) + _ctx.StoredVelocity.x) / totalTimeInAir;

        
        if(_ctx.Rb.velocity.y < 0){
            _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x, 0);
        }
        

        _ctx.Rb.velocity += new Vector2(horizontalJumpForce * _ctx.XInput , verticalJumpForce);  

    }




}