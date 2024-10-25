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


        _targetSpeed = Mathf.Abs(_ctx.StoredVelocity.x) + _ctx.PlayerData.MaxSpeed + (2*Mathf.Sqrt(Mathf.Abs(_ctx.Rb.velocity.y)));

        bool needsDeceleration = _ctx.XInput != 0 &&
                                 Mathf.Abs(_ctx.Rb.velocity.x) < 0.5f &&
                                 (Mathf.Sign(_ctx.XInput) != Mathf.Sign(_ctx.Rb.velocity.x) ||
                                  Mathf.Sign(_ctx.XInput) != Mathf.Sign(_oldXInput));

        if (needsDeceleration && !hasDecelerated)
        {
            Deceleration();
            hasDecelerated = true;  // Set flag to prevent repeated deceleration
        }else{
            if(Mathf.Abs(_ctx.Rb.velocity.x) > _targetSpeed ){
                Run();
            }else{
                Acceleration();
            }
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
            Deceleration();
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

    private void Deceleration()
    {
        _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x / 3, _ctx.Rb.velocity.y);
    }

}