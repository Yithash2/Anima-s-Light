using UnityEngine;

public class IddleState : BaseState
{
    public IddleState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){}
    public override void EnterState()
    {

    }

    public override void UpdateState(){
        
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