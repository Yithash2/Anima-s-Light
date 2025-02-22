using UnityEngine;

public class AirState : BaseState
{

    public AirState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        
        _isRootState = true;
    }

    bool _hasCeilled;

    public override void InitialiseSubState()
    {   
        if( _ctx.JumpPressed && _ctx.XInput != 0 && _ctx.CanDashInAir()){
            SetSubState(_factory.Dash()); 
        }else if(_ctx.DivePressed && _ctx.Rb.velocity.y < 0){
            SetSubState(_factory.Dive());
        }else if(_ctx.XInput != 0){
            SetSubState(_factory.Controlling()); 
        }else{
            SetSubState(_factory.Fall());
        }
        
    }

    public override void EnterState()
    {

        
    }

    public override void UpdateState()
    { 

        _ctx.YCamOffcet = Mathf.Lerp(_ctx.YCamOffcet, -3, 0.05f);


        if(_ctx.IsCeilled() && !_hasCeilled  && _ctx.JumpPressed){
            _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x* 2f, -_ctx.Rb.velocity.y* 1.5f); 
            _hasCeilled = true;
        }
        

        _ctx.AccelerationTime = Mathf.Lerp(_ctx.AccelerationTime, _ctx.PlayerData.AirAccelTime, 0.2f);

        _ctx.UpdateDashTimer();

        CheckSwitchState();


    }

    public override void QuitState()
    {
        _hasCeilled = false;
    }

    public override void CheckSwitchState()
    {
        if(_ctx.IsSpaced()){
            SwitchState(_factory.Wet());
        }else if(_ctx.IsGrounded()){
            SwitchState(_factory.Grounded());
        }else if(_ctx.IsWalled() && (_ctx.IsWallJumping || (_ctx.XInput != 0 && -Mathf.Sign(_ctx.XInput) == Mathf.Sign(_ctx.transform.localScale.x)))){
            SwitchState(_factory.Walled());
        }
    }

    
}
