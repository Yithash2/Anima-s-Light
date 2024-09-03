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

        if(_ctx.JumpPressed && _ctx.DivePressed && _ctx.XInput != 0) SwitchState(_factory.Dash());
        else if(_ctx.JumpPressed && _ctx.CanJump()) SwitchState(_factory.JumpStart());
        else if(_ctx.XInput != 0) SwitchState(_factory.Running());
        
        
    }

    public override void InitialiseSubState()
    {
        
    }
}