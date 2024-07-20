using UnityEngine;

public class SpaceState : BaseState
{

    public SpaceState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        InitialiseSubState();
        _isRootState = true;
    }

    public override void InitialiseSubState()
    {
        if(_ctx.XInput != 0){
            SetSubState(_factory.Running());
        }else{
            SetSubState(_factory.Iddle());
        }
    }

    public override void EnterState()
    {
        _ctx.Rb.drag = 7;
    }

    public override void UpdateState()
    {
        _ctx.JumpCount = 0;
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
        }else if(_ctx.YInput < 0 && !_ctx.IsGrounded()){
            SwitchState(_factory.GroundSlam());
        }else if(!_ctx.IsSpaced() && _ctx.IsGrounded()){
            SwitchState(_factory.Grounded());
        }else if(!_ctx.IsSpaced() && _ctx.Rb.velocity.y < 0){
            SwitchState(_factory.Falling());
        }
    }

    
}
