using UnityEngine;

public class RunState : BaseState{
    public RunState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){}

    float _targetSpeed;
    private int _previousXInput;
    

    public override void EnterState()
    {
        if (_ctx.XInput != 0)
            _previousXInput = (int)Mathf.Sign(_ctx.XInput);

    }

    public override void UpdateState(){
        

        _targetSpeed = Mathf.Abs(_ctx.StoredVelocity.x) + _ctx.PlayerData.MaxSpeed + _ctx.currentSpeedModifier;
        if (HasChangedDirection())
        {
            Debug.Log("a");
            _ctx.Deceleration();
        }
        else if (Mathf.Abs(_ctx.Rb.velocity.x) < _targetSpeed)
        {
            Debug.Log("b");
            
            _ctx.Acceleration(_targetSpeed * _ctx.XInput);
        }
        else
        {
            Debug.Log("c");
            
            _ctx.Run(_targetSpeed * Mathf.Sign(_ctx.Rb.velocity.x)); // clamp / stabilize speed
        }
        

        CheckSwitchState();

        if (_ctx.XInput != 0)
            _previousXInput = (int)Mathf.Sign(_ctx.XInput);

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
        return _ctx.Rb.velocity.x == 0 && (int)Mathf.Sign(_ctx.XInput) != (int)Mathf.Sign(_ctx.Rb.velocity.x);
    }
}