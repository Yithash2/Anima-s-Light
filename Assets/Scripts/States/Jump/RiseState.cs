using UnityEngine;

public class RiseState : BaseState{

    public RiseState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){}

    float _targetSpeed;

    bool _decelerated;

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
        if(_ctx.XInput != 0){
            SwitchState(_factory.JumpControlling());
        }
    }

    public override void InitialiseSubState()
    {
        
    }

}