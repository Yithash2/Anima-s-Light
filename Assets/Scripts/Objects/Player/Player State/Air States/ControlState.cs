using UnityEngine;

public class ControlState : BaseState
{
    public ControlState(PlayerStateMachine currentContext, StateFactory stateFactory)
        : base(currentContext, stateFactory) {}

    private float _targetSpeed;
    private float _oldXInput;
    private bool needDecleration;

    public override void EnterState()
    {
        if (!_ctx.IsAnimating("Respawn", _ctx.Animator)) _ctx.Animator.Play(_ctx.Animations[1].name);
        _ctx.Rb.gravityScale = _ctx.GravityScale * _ctx.PlayerData.GravityMult;
        if (_ctx.XInput != 0)
            _oldXInput = (int)Mathf.Sign(_ctx.XInput);
        needDecleration = false;  // Reset deceleration flag on entering the state
    }

    public override void UpdateState()
    {

        _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x, Mathf.Clamp(_ctx.Rb.velocity.y, -_ctx.PlayerData.ClampedVelocity, float.MaxValue));
        
        _targetSpeed = Mathf.Abs(_ctx.StoredVelocity.x) + _ctx.PlayerData.MaxSpeed + _ctx.currentSpeedModifier;
        
        if (HasChangedDirection() || needDecleration)
        {
            needDecleration = true;
            _ctx.Deceleration();

            if (Mathf.Abs(_ctx.Rb.velocity.x) < 10f)
            {
                needDecleration = false;
            }
        }
        else if (Mathf.Abs(_ctx.Rb.velocity.x) < _targetSpeed)
        {
            _ctx.Acceleration(_targetSpeed * _ctx.XInput);
        }
        else 
        {
            _ctx.Run(_targetSpeed * Mathf.Sign(_ctx.Rb.velocity.x)); // clamp / stabilize speed
        }

        CheckSwitchState();
        
        if (_ctx.XInput != 0)
            _oldXInput = (int)Mathf.Sign(_ctx.XInput);

    }

    public override void QuitState()
    {
        needDecleration = false;  // Ensure flag resets on quitting the state
    }

    public override void CheckSwitchState()
    {
        if (_ctx.JumpPressed && _ctx.XInput != 0 && _ctx.CanDashInAir())
        {
            SwitchState(_factory.Dash());
            return;
        }
        if (_ctx.DivePressed)
        {
            SwitchState(_factory.Dive());
            return;
        }
        if (_ctx.XInput == 0)
        {
            SwitchState(_factory.Fall());
            return;
        }

        if (_currentSuperState is WalledState)
        {
            _currentSuperState.InitialiseSubState();
            return;
        }
    }

    public override void InitialiseSubState() {}
    
    private bool HasChangedDirection()
    {
        return _ctx.XInput != 0 &&
               (int)Mathf.Sign(_ctx.XInput) != (int)Mathf.Sign(_ctx.Rb.velocity.x) &&
               Mathf.Abs(_ctx.Rb.velocity.x) > 10f;
    }
    
}
