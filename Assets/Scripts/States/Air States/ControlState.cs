using UnityEngine;

public class ControlState : BaseState{

    public ControlState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){}

    float _targetSpeed;

    bool _decelerated;

    public override void EnterState()
    {
        _decelerated = false;
        
    }

    public override void UpdateState(){

        _ctx.AccelerationTime = Mathf.Lerp(_ctx.AccelerationTime, _ctx.PlayerData.AirAccelTime, 0.2f);

        _targetSpeed = Mathf.Abs(_ctx.StoredVelocity.x) + _ctx.PlayerData.MaxSpeed;

        if(_ctx.XInput != 0){
            if(Mathf.Abs(_ctx.Rb.velocity.x) > _targetSpeed ){
                Run();
            }else{
                Acceleration();
            }
        }


        CheckSwitchState();
    }

    public override void QuitState()
    {
        
    }

    public override void CheckSwitchState()
    {
        if(_ctx.DivePressed && _ctx.Rb.velocity.y < 0){
            SwitchState(_factory.Dive());
        }else if(_ctx.XInput == 0){
            Deceleration();
            SwitchState(_factory.Fall());
        }
    }

    public override void InitialiseSubState()
    {
        
    }

    void Run(){
        
        Vector2 _direction = new Vector2(Mathf.Sign(_ctx.Rb.velocity.x) * _targetSpeed, _ctx.Rb.velocity.y);
        
        _ctx.Rb.velocity = new Vector2(Mathf.Lerp(_ctx.Rb.velocity.x, _direction.x, 0.00002f * Time.deltaTime), _direction.y);
    }

    void Acceleration(){
        Vector2 _direction = new Vector2(_ctx.XInput, 0) * (_targetSpeed / _ctx.AccelerationTime);
        _ctx.Rb.AddForce(_direction);
    }

    void Deceleration(){
        _decelerated = true;
        _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x/3, _ctx.Rb.velocity.y);   
    }
}