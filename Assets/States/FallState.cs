using UnityEngine;

public class FallState : BaseState{
    
    public FallState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        InitialiseSubState();
        _isRootState = true;
    }
    public override void EnterState()
    {
        Debug.Log("Falling");
        _ctx.Rb.gravityScale = _ctx.GravityScale * _ctx.GravityMult;
    }

    public override void UpdateState(){
        Mathf.Clamp(_ctx.Rb.velocity.y, -_ctx.ClampedVelocity, _ctx.ClampedVelocity);

        CheckSwitchState();
    }

    public override void QuitState()
    {
        
    }

    public override void CheckSwitchState()
    {
        if(_ctx.IsGounded){
            SwitchState(_factory.Grounded());
        }
    }

    public override void InitialiseSubState()
    {
        if(_ctx.xInput != 0){
            SetSubState(_factory.Running());
        }else{
            SetSubState(_factory.Iddle());
        }
    }
}