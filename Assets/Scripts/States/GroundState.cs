using System.Runtime.CompilerServices;
using UnityEngine;

public class GroundState : BaseState{

    public GroundState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        InitialiseSubState();
        _isRootState = true;
    }
    
    public override void EnterState()
    {
        _ctx.Animator.Play(_ctx.Animations[0].name);

        _ctx.Rb.gravityScale = _ctx.GravityScale;

        _ctx.YCamOffcet = 2;

        _ctx.BufferCounter = 0;
        
        _ctx.BufferCounter = 0;
        _ctx.WallJumpCounter = 1000000000;
    }

    public override void UpdateState(){

        _ctx.AccelerationTime = _ctx.PlayerData.GroundAccelTime;

        if(_ctx.PlatformVelocityY < 0){
            _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x, _ctx.PlatformVelocityY);
        if(!_ctx.IsOnMovingPlatform){
            _ctx.Rb.drag = 0.5f;
            _ctx.StoredVelocity = Vector2.zero;
            _ctx.PlatformVelocityX = 0;
            _ctx.PlatformVelocityY = 0;
        }

        CheckSwitchState();

    }

    public override void QuitState()
    {
        _ctx.Rb.drag = 0;
    }

    public override void CheckSwitchState()
    {
        if(_ctx.JumpPressed && _ctx.CanJump()){
            SwitchState(_factory.Jumping());
        }else if(_ctx.IsSpaced()){
            SwitchState(_factory.Spaced());
        }else if(!_ctx.IsOnMovingPlatform && !_ctx.IsGrounded() && _ctx.Rb.velocity.y < 0){
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
}