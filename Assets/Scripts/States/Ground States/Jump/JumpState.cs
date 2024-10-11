using UnityEngine;
using System.Collections;
public class JumpState : BaseState{
    
    public JumpState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        _stateTimer = 1;
    }

    float jumpForce;
    public override void EnterState()
    {

        InitialiseSubState();

        _stateTimeCounter = 0;

        if(!_ctx.IsAnimating("Respawn")) _ctx.Animator.Play(_ctx.Animations[2].name);
        _ctx.AudioSource.clip = _ctx.AudioClips[0];
        _ctx.Rb.gravityScale = _ctx.GravityScale * _ctx.PlayerData.GravityMult;

        _ctx.AudioSource.Play();

        _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x, 0);

        HandleJumpStart();
        
    }

    public override void UpdateState(){
         _ctx.AccelerationTime = Mathf.Lerp(_ctx.AccelerationTime, _ctx.PlayerData.AirAccelTime, 0.2f);

        if(_stateTimeCounter < _stateTimer && (_ctx.JumpReleased || _ctx.Rb.velocity.y < 0)){
            _stateTimeCounter = _stateTimer + 1;
            HandleJumpEnd();
        }

        CheckSwitchState();
        
    }

    public override void QuitState()
    {
        _stateTimeCounter = _stateTimer + 1;
    }

    public override void CheckSwitchState()
    {   
        if(_ctx.IsWalled() && Mathf.Sqrt(Mathf.Abs(6*_ctx.Rb.velocity.y)) < Mathf.Abs(_ctx.Rb.velocity.x) && _ctx.XInput == 0 && !_ctx.JumpReleased){
            _stateTimeCounter = _stateTimer;
            _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x,_ctx.JumpPower);
            SwitchState(_factory.WallJump());
        }else if(_stateTimeCounter >= _stateTimer){
           if(_ctx.XInput == 0){
                SwitchState(_factory.Iddle());
            }else{
                SwitchState(_factory.Running());
            }
        }
        
    }

    public override void InitialiseSubState()
    {
        if(_ctx.XInput == 0){
            SetSubState(_factory.Rising());
        }else{
            SetSubState(_factory.JumpControlling());
        }
    }

    void HandleJumpStart(){

        _ctx.JumpReleased = false;
        _ctx.BufferCounter = 0;
        _ctx.JumpCount ++;
        _ctx.CoyoteCounter = _ctx.CoyoteTime + 1;

        _ctx.IsJumping = true;

        
        float gravity = Physics2D.gravity.y * _ctx.Rb.gravityScale;

        if(_ctx.StoredVelocity.y >= 0){
            _ctx.JumpPower = Mathf.Sqrt(_ctx.PlayerData.InitalJumpHeight * -2 * gravity)  + _ctx.StoredVelocity.y;
        }else if(_ctx.PlatformVelocityY != 0){
            float a = _ctx.StoredVelocity.y * Mathf.Sqrt(_ctx.PlayerData.InitalJumpHeight * -2 * gravity);
            _ctx.JumpPower = Mathf.Sqrt((_ctx.PlayerData.InitalJumpHeight - a) * -2 * gravity) - Mathf.Sqrt(Mathf.Abs(a) * -2 * gravity);
        }else{
            _ctx.JumpPower = Mathf.Sqrt(_ctx.PlayerData.InitalJumpHeight * -2 * gravity);
        }


            
        _ctx.Rb.velocity += new Vector2(0, _ctx.JumpPower);
        
        
        
        _ctx.JumpPressed = false;
    }

    void HandleJumpEnd(){
        _ctx.IsJumping = false;
        _ctx.JumpReleased = false;
        _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x,_ctx.Rb.velocity.y/3);
    }

    





}