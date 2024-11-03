using UnityEngine;

public class RunState : BaseState{
    public RunState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){}

    float _targetSpeed;
    private int _previousXInput;

    float _currentSpeed;

    public override void EnterState()
    {
        _previousXInput = (int)Mathf.Sign(_ctx.XInput);
        _currentSpeed = _ctx.Rb.velocity.x;
    }

    public override void UpdateState(){
        

        _targetSpeed = Mathf.Abs(_ctx.StoredVelocity.x) + _ctx.PlayerData.MaxSpeed;
        if (HasChangedDirection())
        {
            Deceleration();
            
        }
        else if(Mathf.Abs(_ctx.Rb.velocity.x) > _targetSpeed ){
            Run();
        }else{
            Acceleration();
        }

        CheckSwitchState();

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
            Deceleration();
            SwitchState(_factory.Iddle());
        }
        
    }

    public override void InitialiseSubState()
    {
        
    }

    void Run(){
        
        Vector2 _direction = new Vector2(Mathf.Sign(_ctx.Rb.velocity.x) * _targetSpeed, _ctx.Rb.velocity.y);

        if(Mathf.Abs(_currentSpeed) <= 10){
            _ctx.Rb.velocity = new Vector2(Mathf.Lerp(_ctx.Rb.velocity.x, _direction.x, 0.5f * Time.deltaTime), _ctx.Rb.velocity.y);
        }else{
            _ctx.Rb.velocity = new Vector2(Mathf.Lerp(_ctx.Rb.velocity.x, _direction.x, 50/Mathf.Pow(_currentSpeed, 2)* Time.deltaTime), _ctx.Rb.velocity.y);
        }
    }

    void Acceleration(){
        Vector2 _direction = new Vector2(_ctx.XInput, 0) * (_targetSpeed / _ctx.AccelerationTime);
        _ctx.Rb.AddForce(_direction);
    }

    void Deceleration(){
        _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x/1.25f, _ctx.Rb.velocity.y);   
    }

    private bool HasChangedDirection()
    {
        return _ctx.XInput != 0 && Mathf.Sign(_ctx.XInput) != _previousXInput;
    }
}