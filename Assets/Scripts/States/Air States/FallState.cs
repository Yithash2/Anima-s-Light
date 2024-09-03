using UnityEngine;

public class FallState : BaseState{

    public FallState(PlayerStateMachine currentContext, StateFactory stateFactory)
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
        if(_ctx.DivePressed && _ctx.Rb.velocity.y < 0){
            SwitchState(_factory.Dive());
        }else if(_ctx.JumpReleased && _ctx.Rb.velocity.y > 0 && _ctx.IsJumping){
            SwitchState(_factory.JumpEnd());
        }else if(_ctx.XInput != 0){
            SwitchState(_factory.Controlling());
        }
    }

    public override void InitialiseSubState()
    {
        
    }

}