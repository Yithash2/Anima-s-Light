using UnityEngine;

public class IddleState : BaseState
{
    public IddleState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){}
    


    public override void EnterState()
    {
        _ctx._camera.ResetPostion();
    }

    public override void UpdateState(){
        if(Mathf.Abs(_ctx.Rb.linearVelocity.x) > 0 && Mathf.Abs(_ctx.Rb.linearVelocity.x) <= _ctx.PlayerData.MaxSpeed + _ctx.currentSpeedModifier){
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

        if(_ctx.JumpPressed && _ctx.CanJump()){
            SwitchState(_factory.JumpStart());
            }
        else if(_ctx.XInput != 0){
            SwitchState(_factory.Running());
        }else if(_ctx.JumpPressed && _ctx.DivePressed) SwitchState(_factory.Dash());
        
         
        
        
    }

    public override void InitialiseSubState()
    {
        
    }
}