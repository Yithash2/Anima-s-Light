using UnityEngine;

public class AirState : BaseState
{

    public AirState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        
        _isRootState = true;
    }

    public override void InitialiseSubState()
    {
        if(_ctx.DivePressed && _ctx.Rb.velocity.y < 0){
            SetSubState(_factory.Dive());
        }else if(_ctx.XInput != 0){
            SetSubState(_factory.Controlling()); 
        }else{
            SetSubState(_factory.Fall());
        }
        
    }

    public override void EnterState()
    {

        InitialiseSubState();
        
    }

    public override void UpdateState()
    { 

        _ctx.YCamOffcet = Mathf.Lerp(_ctx.YCamOffcet, -3, 0.5f);

        _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x, Mathf.Clamp(_ctx.Rb.velocity.y, -_ctx.PlayerData.ClampedVelocity, float.MaxValue));

        _ctx.AccelerationTime = Mathf.Lerp(_ctx.AccelerationTime, _ctx.PlayerData.AirAccelTime, 0.2f);

        CheckSwitchState();

    }

    public override void QuitState()
    {

    }

    public override void CheckSwitchState()
    {
        if(_ctx.IsSpaced()){
            SwitchState(_factory.Wet());
        }else if(_ctx.IsGrounded()){
            SwitchState(_factory.Grounded());
        }else if(_ctx.IsWalled()){
            SwitchState(_factory.Walled());
        }
    }

    
}
