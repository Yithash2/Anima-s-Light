using UnityEngine;

public class SwimState : BaseState{

    public SwimState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){}

    float _targetSpeed;

    public override void EnterState()
    {
    }

    public override void UpdateState(){

        _targetSpeed = Mathf.Abs(_ctx.StoredVelocity.x) + _ctx.PlayerData.MaxSpeed;

        if(_ctx.XInput != 0){
            if(Mathf.Abs(_ctx.Rb.velocity.x) > _targetSpeed ){
                Run();
            }else if(_ctx.XInput != _ctx.PlayerTransform.localScale.x){
                Acceleration();
            }else{
                Deceleration();
            }
        }

        if(_ctx.JumpPressed){
            HandleSwimUp();
        }else{
            HandleSwimStop();
        }

        
        

        CheckSwitchState();
    }

    public override void QuitState()
    {

    }

    public override void CheckSwitchState()
    {
        if(_ctx.XInput == 0 && !_ctx.JumpPressed){
            SwitchState(_factory.Sinking());
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
        _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x/3, _ctx.Rb.velocity.y);   
    }

    void HandleSwimUp(){
        _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x, 10);
    }
    
    void HandleSwimStop(){
         _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x, _ctx.Rb.velocity.y/3);
    }
}