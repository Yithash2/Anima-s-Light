using UnityEngine;

public class SinkState : BaseState
{
    public SinkState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){}

    

    public override void EnterState()
    {

    }

    public override void UpdateState(){
        if(Mathf.Abs(_ctx.Rb.velocity.x) > 0 && Mathf.Abs(_ctx.Rb.velocity.x) <=  _ctx.PlayerData.MaxSpeed + _ctx.currentSpeedModifier){
            _ctx.Deceleration();
        }else if(Mathf.Abs(_ctx.Rb.velocity.x) > 0){
            _ctx.Deceleration(0.1f);
        }
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