using UnityEngine;
using System.Collections;
public class JumpStartState : BaseState{
    
    public JumpStartState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        _stateTimer = 1;
    }

    float jumpForce;
    public override void EnterState()
    {

        InitialiseSubState();

        if(!_ctx.IsAnimating("Respawn")) _ctx.Animator.Play(_ctx.Animations[2].name);
        _ctx.AudioSource.clip = _ctx.AudioClips[0];
        _ctx.Rb.gravityScale = _ctx.GravityScale * _ctx.PlayerData.GravityMult;

        _ctx.AudioSource.Play();

        _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x, 0);

        HandleJumpStart();
        
    }

    public override void UpdateState(){
        Debug.Log(_stateTimeCounter);
        if(_ctx.JumpReleased || _ctx.Rb.velocity.y < 0){
            _stateTimeCounter = _stateTimer;
        }

        CheckSwitchState();
        
    }

    public override void QuitState()
    {
        
    }

    public override void CheckSwitchState()
    {   
        if(_ctx.IsWalled() && Mathf.Sqrt(Mathf.Abs(5*_ctx.Rb.velocity.y)) < Mathf.Abs(_ctx.Rb.velocity.x) && !_ctx.JumpReleased){
            _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x,_ctx.JumpPower*2);
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