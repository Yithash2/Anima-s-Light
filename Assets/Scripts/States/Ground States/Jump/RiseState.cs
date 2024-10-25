using UnityEngine;

public class RiseState : BaseState{

    public RiseState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){}


    public override void EnterState()
    {

    }

    public override void UpdateState(){
        _ctx.Rb.velocity = new Vector2(Mathf.Lerp(_ctx.Rb.velocity.x, 0, 0.05f), _ctx.Rb.velocity.y);
        CheckSwitchState();
    }

    public override void QuitState()
    {

    }

    public override void CheckSwitchState()
    {
        if(_ctx.XInput != 0){
            SwitchState(_factory.JumpControlling());
        }
    }

    public override void InitialiseSubState()
    {
        
    }

}