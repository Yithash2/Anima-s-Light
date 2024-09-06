using UnityEngine;

public class IddleState : BaseState
{
    public IddleState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){}

    

    public override void EnterState()
    {
        _ctx.AudioSource.loop = false;

        _ctx.AudioSource.clip = null;
    }

    public override void UpdateState(){
        CheckSwitchState();
    }

    public override void QuitState()
    {

    }

    public override void CheckSwitchState()
    {

        if(_ctx.JumpPressed && _ctx.CanJump()) SwitchState(_factory.JumpStart());
        else if(_ctx.XInput != 0){
            Debug.Log("D");
            SwitchState(_factory.Running());
        }else if(_ctx.JumpPressed && _ctx.DivePressed) SwitchState(_factory.Dash());
        
         
        
        
    }

    public override void InitialiseSubState()
    {
        
    }
}