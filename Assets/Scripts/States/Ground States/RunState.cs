using UnityEngine;

public class RunState : BaseState{
    public RunState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){}

    float _targetSpeed;

    public override void EnterState()
    {
    }

    public override void UpdateState(){

        _targetSpeed = Mathf.Abs(_ctx.StoredVelocity.x) + _ctx.PlayerData.MaxSpeed;

        if(Mathf.Abs(_ctx.Rb.velocity.x) > _targetSpeed ){
            Run();
        }else{
            Acceleration();
        }



        
        

        CheckSwitchState();
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
        }else if(_ctx.XInput == 0 ){
            Deceleration();
            SwitchState(_factory.Iddle());
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
        _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x/1.25f, _ctx.Rb.velocity.y);   
    }
}