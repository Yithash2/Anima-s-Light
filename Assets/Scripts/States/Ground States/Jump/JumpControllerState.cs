using UnityEngine;
using UnityEngineInternal;

public class JumpControllerState : BaseState{

    public JumpControllerState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){}

    float _targetSpeed;


    public override void EnterState()
    {
    }

    public override void UpdateState(){


        _targetSpeed = Mathf.Abs(_ctx.StoredVelocity.x) + _ctx.PlayerData.MaxSpeed + (2*Mathf.Sqrt(Mathf.Abs(_ctx.Rb.velocity.y)));

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
        if(_ctx.XInput == 0 || Mathf.Sign(_ctx.XInput) != Mathf.Sign(_ctx.Rb.velocity.x)){
            SwitchState(_factory.Rising());
            _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x /3, _ctx.Rb.velocity.y);
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

}