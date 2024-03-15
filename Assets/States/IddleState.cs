using UnityEngine;

public class IddleState : BaseState
{
    public IddleState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){}
    public override void EnterState()
    {
        _ctx.Animator.Play(_ctx.Animations[0].name);
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
            SwitchState(_factory.Running());
        }
    }

    public override void InitialiseSubState()
    {
        
    }
}