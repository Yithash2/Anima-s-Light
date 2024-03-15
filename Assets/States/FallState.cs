using UnityEngine;

public class FallState : BaseState{
    
    public FallState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        InitialiseSubState();
        _isRootState = true;
    }
    public override void EnterState()
    {
        _ctx.Animator.Play(_ctx.Animations[1].name);
        _ctx.Rb.gravityScale = _ctx.GravityScale * _ctx.GravityMult;

    }

    public override void UpdateState(){
        _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x, Mathf.Clamp(_ctx.Rb.velocity.y, -_ctx.ClampedVelocity, _ctx.ClampedVelocity));

        _ctx.AccelRate = (Mathf.Abs(_ctx.TargetSpeed) > 0.01f) ? _ctx.RunAccelAmount * _ctx.AirMult : _ctx.RunDeccelAmount * _ctx.AirMult;

        CheckSwitchState();
    }

    public override void QuitState()
    {
        
    }

    public override void CheckSwitchState()
    {
        if(_ctx.YInput < 0){
            SwitchState(_factory.GroundSlam());
        }else if(_ctx.IsGounded){
            SwitchState(_factory.Grounded());
        }else if(_ctx.isWalled){
            SwitchState(_factory.Walled());
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