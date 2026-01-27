using UnityEngine;
using UnityEngineInternal;

public class JumpControllerState : BaseState{

    public JumpControllerState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){}

    float _targetSpeed;
    private float _oldXInput;
    private bool hasDecelerated;


    public override void EnterState()
    {
        _oldXInput = _ctx.XInput;
    }

    public override void UpdateState(){


        _targetSpeed = Mathf.Abs(_ctx.StoredVelocity.x) + _ctx.PlayerData.MaxSpeed + (Mathf.Sqrt(Mathf.Abs(_ctx.Rb.velocity.y))) + _ctx.currentSpeedModifier;
        
        if (HasChangedDirection())
        {
            _ctx.Deceleration();
        }
        else if (Mathf.Abs(_ctx.Rb.velocity.x) < _targetSpeed)
        {
            _ctx.Acceleration(_targetSpeed * _ctx.XInput);
        }
        else
        {
            _ctx.Run(_targetSpeed * Mathf.Sign(_ctx.Rb.velocity.x)); // clamp / stabilize speed
        }


        CheckSwitchState();

        _oldXInput = _ctx.XInput;
    }

    public override void QuitState()
    {
    }

    public override void CheckSwitchState()
    {
        if(_ctx.XInput == 0){
            SwitchState(_factory.Rising());
        }
    }

    public override void InitialiseSubState()
    {
        
    }
    
    
    private bool HasChangedDirection()
    {
        return Mathf.Sign(_ctx.XInput) != _oldXInput;
    }

}