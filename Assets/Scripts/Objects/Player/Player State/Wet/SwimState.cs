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

        CheckSwitchState();

        _targetSpeed = Mathf.Abs(_ctx.StoredVelocity.x) + _ctx.PlayerData.MaxSpeed + _ctx.currentSpeedModifier;
        if (HasChangedDirection())
        {
            _ctx.Deceleration();
        }
        else if (Mathf.Abs(_ctx.Rb.linearVelocity.x) < _targetSpeed)
        {
            _ctx.Acceleration(_targetSpeed * _ctx.XInput);
        }
        else
        {
            _ctx.Run(_targetSpeed * Mathf.Sign(_ctx.Rb.linearVelocity.x)); // clamp / stabilize speed
        }

        if (_ctx.JumpPressed) PressSwim();
        if (!_ctx.JumpReleased || _ctx.DivePressed) Swim();


        _oldXInput = _ctx.XInput;
    }

    public override void QuitState()
    {

    }

    public override void CheckSwitchState()
    {

        if (_ctx.DivePressed && _ctx.XInput != 0 && _ctx.JumpPressed && _ctx.IsTouchingGround())
        {
            SwitchState(_factory.SeaDash());
        }else if(_ctx.XInput == 0 && _ctx.JumpReleased && !_ctx.DivePressed)
        {
            SwitchState(_factory.Sinking());
        }
    }

    public override void InitialiseSubState()
    {
        
    }
    
    void Swim()
    {

        if (_ctx.Rb.linearVelocity.y >= _ctx.PlayerData.MaxSpeed)
        {
            SwimNormaly();
        }
        else
        {
            AccelerateSwim();
        }
    }

    void AccelerateSwim()
    {
        float yInput = 0;
        if (!_ctx.JumpReleased) yInput = 1;
        else if (_ctx.DivePressed) yInput = -1;

        Vector2 _direction = yInput * Vector2.up * (_ctx.PlayerData.MaxSpeed / _ctx.AccelerationTime);
        _ctx.Rb.linearVelocity += _direction * Time.fixedDeltaTime;

    }

    void SwimNormaly()
    {
        float yInput = 0;
        if (!_ctx.JumpReleased) yInput = 1;
        else if(_ctx.DivePressed) yInput = -1;

        _ctx.Rb.linearVelocity = Vector2.Lerp(_ctx.Rb.linearVelocity, new Vector2(_ctx.Rb.linearVelocity.x, yInput * _ctx.PlayerData.MaxSpeed), 0.00002f * Time.deltaTime);
    }

    void PressSwim()
    {
        float gravity = Physics2D.gravity.y * _ctx.Rb.gravityScale;

        _ctx.JumpPower = Mathf.Sqrt(_ctx.PlayerData.InitalJumpHeight * -2 * gravity) + _ctx.StoredVelocity.y;

        _ctx.Rb.linearVelocity += new Vector2(0, _ctx.JumpPower);

        _ctx.JumpPressed = false;
    }
    
    private bool HasChangedDirection()
    {
        return Mathf.Sign(_ctx.XInput) != _oldXInput;
    }

}