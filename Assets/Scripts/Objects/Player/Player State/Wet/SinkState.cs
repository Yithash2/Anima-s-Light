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
        if (_ctx.DivePressed && _ctx.XInput != 0 && _ctx.JumpPressed && _ctx.IsTouchingGround())
        {
            SwitchState(_factory.SeaDash());
        }
        else if (_ctx.XInput != 0 || !_ctx.JumpReleased || _ctx.DivePressed) { 
            
            SwitchState(_factory.Swimming()); 
        }
        
    }

    public override void InitialiseSubState()
    {
        
    }
}