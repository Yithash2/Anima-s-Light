using UnityEngine;

public class RapelState : BaseState{

    public RapelState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        _canTurn = false;
    }

    float _targetSpeed;

    public override void EnterState()
    {
        _ctx.WallJumpCounter = 0;
        _ctx.Rb.linearVelocity = new Vector2(0, _ctx.Rb.linearVelocity.y);
        
    }

    public override void UpdateState(){

        _targetSpeed = Mathf.Abs(_ctx.StoredVelocity.x) + _ctx.PlayerData.MaxSpeed;

        float coef = 5;
        _ctx.Rb.linearVelocity = new Vector2(_ctx.Rb.linearVelocity.x, Mathf.Clamp(_ctx.Rb.linearVelocity.y, 
            -(coef *_ctx.PlayerData.SlideSpeed / ((coef-1)*Mathf.Abs(_ctx.GroundType.friction) + 1)) + _ctx.PlatformVelocityY, float.MaxValue));

        
        

        CheckSwitchState();
    }

    public override void QuitState()
    {
        _ctx.WallJumpCounter = _ctx.PlayerData.WallJumpControlTime + 1;
    }

    public override void CheckSwitchState()
    {
        if(_ctx.DivePressed){
            SwitchState(_factory.Dive());
        }else if(_ctx.JumpPressed){
            SwitchState(_factory.WallJump());
        }
    }

    public override void InitialiseSubState()
    {
        
    }

    void Run(){
        
        Vector2 _direction = new Vector2(Mathf.Sign(_ctx.Rb.linearVelocity.x) * _targetSpeed, _ctx.Rb.linearVelocity.y);
        
        _ctx.Rb.linearVelocity = new Vector2(Mathf.Lerp(_ctx.Rb.linearVelocity.x, _direction.x, 0.00002f * Time.deltaTime), _direction.y);
    }

    void Acceleration(){
        Vector2 _direction = new Vector2(_ctx.XInput, 0) * (_targetSpeed / _ctx.AccelerationTime);
        _ctx.Rb.AddForce(_direction);
    }

    void Deceleration(){
        _ctx.Rb.linearVelocity = new Vector2(_ctx.Rb.linearVelocity.x/3, _ctx.Rb.linearVelocity.y);   
    }
}