using UnityEngine;

public class SwimState : BaseState{

    public SwimState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){}

    float _targetSpeed;
    float _oldXInput;
    bool hasDecelerated;

    public override void EnterState()
    {
        _oldXInput = _ctx.XInput;

        hasDecelerated = false;  // Reset deceleration flag on entering the state
    }

    public override void UpdateState(){

        _targetSpeed = Mathf.Abs(_ctx.StoredVelocity.x) + _ctx.PlayerData.MaxSpeed;

        bool needsDeceleration = _ctx.XInput != 0 &&
                                 Mathf.Abs(_ctx.Rb.velocity.x) > 0.5f &&
                                 (Mathf.Sign(_ctx.XInput) != Mathf.Sign(_ctx.Rb.velocity.x) ||
                                  Mathf.Sign(_ctx.XInput) != Mathf.Sign(_oldXInput));

        if (needsDeceleration && !hasDecelerated)
        {
            Deceleration();
            hasDecelerated = true;  // Set flag to prevent repeated deceleration
        }
        else{
            if(Mathf.Abs(_ctx.Rb.velocity.x) > _targetSpeed ){
                Run();
            }else if(_ctx.XInput != _ctx.PlayerTransform.localScale.x){
                Acceleration();
            }else{
                Deceleration();
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

}