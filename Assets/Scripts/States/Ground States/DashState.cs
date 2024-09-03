using UnityEngine;
using System.Collections;
public class DashState : BaseState{
    
    public DashState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        
    }

    float horizontalJumpForce;
    public override void EnterState()
    {

        Debug.Log("0");

        if(!_ctx.IsAnimating("Respawn")) _ctx.Animator.Play(_ctx.Animations[2].name);
        _ctx.AudioSource.clip = _ctx.AudioClips[0]; 

        _ctx.Rb.gravityScale = _ctx.GravityScale * _ctx.PlayerData.GravityMult;

        _ctx.DashCounter = 0;

        _stateFinished = false;

        _ctx.AudioSource.Play();

        HandleJumpStart();
        
    }

    public override void UpdateState(){

        if(_ctx.DashCounter > 0.8 && !_stateFinished) _stateFinished = true;
        else{
            _ctx.DashCounter += Time.deltaTime;
            if(_ctx.DashCounter > 0.4){
                if(_ctx.XInput == _ctx.PlayerTransform.localScale.x)_ctx.Rb.AddForce(new Vector2(15*-_ctx.XInput,0));
                
            }
        }
        
        if(_ctx.IsGrounded()) _stateFinished = true;
        
        _ctx.YCamOffcet = Mathf.Lerp(_ctx.YCamOffcet, 5, 0.125f);
        
        _ctx.Rb.gravityScale = _ctx.GravityScale * _ctx.PlayerData.GravityMult;

        CheckSwitchState();
        
        
    }

    public override void QuitState()
    {
        _stateFinished = true;
        _ctx.DashCounter = 1000000000;

        _ctx.IsDashing = false;

        

    }

    public override void CheckSwitchState()
    {
        if(_ctx.IsWalled() && _ctx.JumpPressed){
            _ctx.Rb.velocity = new Vector2(0, horizontalJumpForce/3);
            SwitchState(_factory.WallJump());
        }else if(_stateFinished){
            if(_ctx.DivePressed && _ctx.JumpPressed && _ctx.IsGrounded() && _ctx.XInput != 0){
                SwitchState(_factory.Dash());
            }else if(_ctx.JumpPressed && _ctx.CanJump()){
                SwitchState(_factory.JumpStart());
            }else if(_ctx.XInput != 0){
                SwitchState(_factory.Running());
            }else{
                SwitchState(_factory.Iddle());
            
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