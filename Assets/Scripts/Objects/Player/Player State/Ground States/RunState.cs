using UnityEngine;

public class RunState : BaseState{
    public RunState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){}

    float _targetSpeed;
    private int _oldXInput;
    private bool needDecleration;
    

    public override void EnterState()
    {
        if (_ctx.XInput != 0)
            _oldXInput = (int)Mathf.Sign(_ctx.XInput);

    }

    public override void UpdateState(){
        

        _targetSpeed = Mathf.Abs(_ctx.StoredVelocity.x) + _ctx.PlayerData.MaxSpeed + _ctx.currentSpeedModifier;
        if (HasChangedDirection() || needDecleration)
        {
            needDecleration = true;
            _ctx.Deceleration();

            if (Mathf.Abs(_ctx.Rb.linearVelocity.x) < 10f)
            {
                needDecleration = false;
            }
        }
        else if (Mathf.Abs(_ctx.Rb.linearVelocity.x) < _targetSpeed)
        {
            _ctx.Acceleration(_targetSpeed * _ctx.XInput);
        }
        else 
        {
            _ctx.Run(_targetSpeed * Mathf.Sign(_ctx.Rb.linearVelocity.x)); // clamp / stabilize speed
        }
        

        CheckSwitchState();

        if (_ctx.XInput != 0)
            _oldXInput = (int)Mathf.Sign(_ctx.XInput);

    }

    public override void QuitState()
    {
    }

    public override void CheckSwitchState()
    {
        
        if(_ctx.JumpPressed && _ctx.DivePressed){
            SwitchState(_factory.Dash());
        }else if(_ctx.JumpPressed && _ctx.CanJump()){
            SwitchState(_factory.JumpStart());
        }else if(_ctx.XInput == 0){
            SwitchState(_factory.Iddle());
        }
        
    }

    public override void InitialiseSubState()
    {
        
    }

    

    private bool HasChangedDirection()
    {
        return _ctx.XInput != 0 &&
               (int)Mathf.Sign(_ctx.XInput) != (int)Mathf.Sign(_ctx.Rb.linearVelocity.x) &&
               Mathf.Abs(_ctx.Rb.linearVelocity.x) > 10f;
    }
}