using UnityEngine;
using System.Collections;
public class DashState : BaseState{
    
    public DashState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        _stateTimer = 0.6f;
        _canTurn = false;
    }

    float horizontalJumpForce;
    public override void EnterState()
    {

        if(!_ctx.IsAnimating("Respawn")) _ctx.Animator.Play(_ctx.Animations[2].name);
        _ctx.AudioSource.clip = _ctx.AudioClips[0]; 

        _ctx.Rb.gravityScale = _ctx.GravityScale * _ctx.PlayerData.GravityMult;

        _stateTimeCounter = 0f;

        _canTurn = false;

        _ctx.DashCounter = 0;

        _ctx.AudioSource.Play();

        HandleJumpStart();
        
    }

    public override void UpdateState(){      

        _stateTimeCounter += Time.fixedDeltaTime;

        if(_stateTimeCounter >= _stateTimer*3/4) _canTurn = true;

        
        _ctx.YCamOffcet = Mathf.Lerp(_ctx.YCamOffcet, 5, 0.125f);
        
        _ctx.Rb.gravityScale = _ctx.GravityScale * _ctx.PlayerData.GravityMult;

        CheckSwitchState();
        
        
    }

    public override void QuitState()
    {

        _ctx.DashCounter = 1000000000;

        _stateTimeCounter = 100f;
        _canTurn = true;

        _ctx.IsDashing = false;

        

    }

    public override void CheckSwitchState()
    {
        if(_ctx.IsWalled()){
            if(!_ctx.JumpPressed){
                _ctx.Rb.velocity = new Vector2(0, horizontalJumpForce/3);
                SwitchState(_factory.WallJump());
            }else{
                SwitchState(_factory.Rapelling());
            }
            
        }else if(_ctx.DivePressed && _ctx.JumpPressed && _ctx.IsGrounded() && _ctx.XInput != 0){ 
                SwitchState(_factory.Dash());
        }else if(_stateTimeCounter >= _stateTimer){
            if(_ctx.IsGrounded()){
                if(_ctx.JumpPressed && _ctx.CanJump()){
                    SwitchState(_factory.JumpStart());
                    Debug.Log("CHERCHER A");
                }else if(_ctx.XInput != 0){
                    SwitchState(_factory.Running());
                }else{
                    //SwitchState(_factory.Iddle()); 
                    Debug.Log("IM A");
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

        _ctx.CoyoteCounter = _ctx.CoyoteTime + 1;
        
        _ctx.BufferCounter = 0;
       
        float gravity = Physics2D.gravity.y * _ctx.Rb.gravityScale;

        float verticalJumpForce = Mathf.Sqrt((3 + _ctx.StoredVelocity.y) * -2 * gravity);
        float timeToPeak = verticalJumpForce / Mathf.Abs(gravity);
        float totalTimeInAir = 2 * timeToPeak;
        horizontalJumpForce = ((20) + _ctx.StoredVelocity.x) / totalTimeInAir;

        
        if((_ctx.Rb.velocity.y < 0)){
            _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x, 0);
        }
        

        _ctx.Rb.velocity += new Vector2(horizontalJumpForce * _ctx.XInput , verticalJumpForce);  

    }




}