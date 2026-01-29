using UnityEngine;

public class RiseState : BaseState{

    public RiseState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){}


    public override void EnterState()
    {

    }

    public override void UpdateState(){
        if(Mathf.Abs(_ctx.Rb.linearVelocity.x) > 0 && Mathf.Abs(_ctx.Rb.linearVelocity.x) <=  _ctx.PlayerData.MaxSpeed + _ctx.currentSpeedModifier){
            _ctx.Deceleration();
        }else if(Mathf.Abs(_ctx.Rb.linearVelocity.x) > 0){
            _ctx.Deceleration(0.5f);
        }
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