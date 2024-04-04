using UnityEngine;

public class IddleState : BaseState
{
    public IddleState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){}
    public override void EnterState()
    {

    }

    public override void UpdateState(){
        if(_ctx.CanMove()){
            _ctx.Rb.velocity = new Vector2(_ctx.BaselineVelocityX, _ctx.Rb.velocity.y);
        }
        
        CheckSwitchState();
    }

    public override void QuitState()
    {
        
    }

    public override void CheckSwitchState()
    {
        if(_ctx.XInput != 0 && _ctx.CanMove()){
            SwitchState(_factory.Running());
        }
    }

    public override void InitialiseSubState()
    {
        
    }
}