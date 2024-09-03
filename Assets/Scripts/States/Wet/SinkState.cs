using UnityEngine;

public class SinkState : BaseState
{
    public SinkState(PlayerStateMachine currentContext, StateFactory stateFactory)
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
        if(_ctx.XInput != 0 || _ctx.JumpPressed) SwitchState(_factory.Swimming());
        
    }

    public override void InitialiseSubState()
    {
        
    }
}