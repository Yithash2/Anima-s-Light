using UnityEngine;
using System.Collections;
public class JumpState : BaseState{
    
    public JumpState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        InitialiseSubState();
        _isRootState = true;
    }
    public override void EnterState()
    {
        
        if(!_ctx.IsAnimating("Respawn")) _ctx.Animator.Play(_ctx.Animations[2].name);
        _ctx.AudioSource.clip = _ctx.AudioClips[0];
        _ctx.Rb.gravityScale = _ctx.GravityScale;

        

        _ctx.AudioSource.Play();

        _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x, 0);

        HandleJumpStart();
        
    }

    public override void UpdateState(){

        _ctx.AccelerationTime = Mathf.Lerp(_ctx.PlayerData.GroundAccelTime, _ctx.PlayerData.AirAccelTime, 0.125f);

        _ctx.YCamOffcet = Mathf.Lerp(_ctx.YCamOffcet, -2, 0.5f);

        if(_ctx.JumpReleased) {
            HandleJumpEnd();
        }
        
        CheckSwitchState();
    }

    public override void QuitState()
    {

        _ctx.IsJumping = false;
       
    }

    public override void CheckSwitchState()
    {
        if(_ctx.IsSpaced()){
            SwitchState(_factory.Spaced());
        }else if(_ctx.IsGrounded()){
            SwitchState(_factory.Grounded());
            _ctx.AudioSource.clip = _ctx.AudioClips[3];
            _ctx.AudioSource.Play();
        }else if(_ctx.IsWalled()){
            SwitchState(_factory.Walled());
        }else if(_ctx.Rb.velocity.y <= 0){
            SwitchState(_factory.Falling());
        }
    }

    public override void InitialiseSubState()
    {
        if(_ctx.XInput != 0){
            SetSubState(_factory.Running());
        }else{
            SetSubState(_factory.Iddle());
        }
    }

    void HandleJumpStart(){

        _ctx.JumpReleased = false;
        _ctx.BufferCounter = 0;
        _ctx.JumpCount ++;
        _ctx.CoyoteCounter = _ctx.CoyoteTime + 1;

        _ctx.IsJumping = true;

        float jumpForce;
        float gravity = Physics2D.gravity.y * _ctx.Rb.gravityScale;

        if(_ctx.StoredVelocity.y >= 0){
            jumpForce = Mathf.Sqrt(_ctx.PlayerData.InitalJumpHeight * -2 * gravity)  + _ctx.StoredVelocity.y;
        }else if(_ctx.PlatformVelocityY != 0){
            float a = _ctx.StoredVelocity.y * Mathf.Sqrt(_ctx.PlayerData.InitalJumpHeight * -2 * gravity);
            jumpForce = Mathf.Sqrt((_ctx.PlayerData.InitalJumpHeight - a) * -2 * gravity) - Mathf.Sqrt(Mathf.Abs(a) * -2 * gravity);
        }else{
            jumpForce = Mathf.Sqrt(_ctx.PlayerData.InitalJumpHeight * -2 * gravity);
        }
            
        _ctx.Rb.velocity += new Vector2(0, jumpForce);
        
        
        
        _ctx.JumpPressed = false;
    }

    void HandleJumpEnd(){
        _ctx.JumpReleased = false;
        _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x,_ctx.Rb.velocity.y/3);
    }


}