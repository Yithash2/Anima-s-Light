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
        
        if(!_ctx.IsAnimating("Respawn")) _ctx.Animator.Play(_ctx.Animations[0].name);

        _ctx.Rb.gravityScale = _ctx.GravityScale;

        

        _ctx.BufferCounter = 0;

    }

    public override void UpdateState(){
        _ctx.YCamOffcet = Mathf.Lerp(_ctx.YCamOffcet, 5, 0.125f);

        _ctx.AccelerationTime = Mathf.Lerp(_ctx.AccelerationTime ,_ctx.PlayerData.GroundAccelTime, 0.2f);

        CheckSwitchState();

        
    }


    public override void QuitState()
    {
    }

    public override void CheckSwitchState()
    {
        if(_ctx.JumpPressed && _ctx.DivePressed && _ctx.XInput != 0){
            SwitchState(_factory.Dash());
        }else if(_ctx.JumpPressed && _ctx.CanJump()){
            SwitchState(_factory.Jumping());
        }else if(_ctx.IsSpaced()){
            SwitchState(_factory.Spaced());
        }else if(!_ctx.IsOnMovingPlatform && !_ctx.IsGrounded() && _ctx.Rb.velocity.y < _ctx.StoredVelocity.y){
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