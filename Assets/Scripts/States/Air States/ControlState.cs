using UnityEngine;

public class ControlState : BaseState
{
    public ControlState(PlayerStateMachine currentContext, StateFactory stateFactory)
        : base(currentContext, stateFactory) {}

    private float _targetSpeed;
    private float _oldXInput;
    private bool hasDecelerated;

    public override void EnterState()
    {
        if (!_ctx.IsAnimating("Respawn")) _ctx.Animator.Play(_ctx.Animations[1].name);
        _ctx.Rb.gravityScale = _ctx.GravityScale * _ctx.PlayerData.GravityMult;
        _oldXInput = _ctx.XInput;
        hasDecelerated = false;  // Reset deceleration flag on entering the state
    }

    public override void UpdateState()
    {
        _ctx.AccelerationTime = Mathf.Lerp(_ctx.AccelerationTime, _ctx.PlayerData.AirAccelTime, 0.2f);
        _targetSpeed = Mathf.Abs(_ctx.StoredVelocity.x) + _ctx.PlayerData.MaxSpeed;

        bool needsDeceleration = _ctx.XInput != 0 &&
                                 Mathf.Abs(_ctx.Rb.velocity.x) < 0.5f &&
                                 (Mathf.Sign(_ctx.XInput) != Mathf.Sign(_ctx.Rb.velocity.x) ||
                                  Mathf.Sign(_ctx.XInput) != Mathf.Sign(_oldXInput));

        if (needsDeceleration && !hasDecelerated)
        {
            Deceleration();
            hasDecelerated = true;  // Set flag to prevent repeated deceleration
        }
        else
        {
            if (Mathf.Abs(_ctx.Rb.velocity.x) > _targetSpeed)
            {
                Run();
            }
            else
            {
                Acceleration();
            }
            hasDecelerated = false;  // Reset flag if no deceleration is applied
        }

        CheckSwitchState();
        _oldXInput = _ctx.XInput;
    }

    public override void QuitState()
    {
        hasDecelerated = false;  // Ensure flag resets on quitting the state
    }

    public override void CheckSwitchState()
    {
        if (_ctx.JumpPressed && _ctx.XInput != 0 && _ctx.CanDashInAir())
        {
            SwitchState(_factory.Dash());
            return;
        }
        else if (_ctx.DivePressed)
        {
            SwitchState(_factory.Dive());
        }
        else if (_ctx.XInput == 0)
        {
            SwitchState(_factory.Fall());
            Deceleration();
        }
    }

    public override void InitialiseSubState() {}

    private void Run()
    {
        Vector2 _direction = new Vector2(Mathf.Sign(_ctx.Rb.velocity.x) * _targetSpeed, _ctx.Rb.velocity.y);
        _ctx.Rb.velocity = new Vector2(Mathf.Lerp(_ctx.Rb.velocity.x, _direction.x, 0.00002f * Time.deltaTime), _direction.y);
    }

    private void Acceleration()
    {
        Vector2 _direction = new Vector2(_ctx.XInput, 0) * (_targetSpeed / _ctx.AccelerationTime);
        _ctx.Rb.AddForce(_direction);
    }

    private void Deceleration()
    {
        _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x / 3, _ctx.Rb.velocity.y);
    }
}
