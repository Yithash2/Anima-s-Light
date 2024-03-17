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

        _ctx.MaxVelocity = _ctx.PlayerData.GroundVelocity;
        
        _ctx.WallJumpCounter = 1000000000;
    }

    public override void UpdateState(){

        _ctx.AccelRate = (Mathf.Abs(_ctx.TargetSpeed) > 0.01f) ? _ctx.RunAccelAmount : _ctx.RunDeccelAmount;

        CheckSwitchState();

    }

    public override void QuitState()
    {
        _ctx.MaxVelocity = _ctx.PlayerData.AirVelocity;
    }

    public override void CheckSwitchState()
    {
        if(_ctx.JumpPressed){
            SwitchState(_factory.Jumping());
        }else if(!_ctx.IsGounded && _ctx.Rb.velocity.y < 0){
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