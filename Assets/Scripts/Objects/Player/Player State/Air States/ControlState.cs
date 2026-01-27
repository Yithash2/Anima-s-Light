using UnityEngine;

public class ControlState : BaseState
{
    public ControlState(PlayerStateMachine currentContext, StateFactory stateFactory)
        : base(currentContext, stateFactory) {}

    private float _targetSpeed;
    private int _oldXInput;
    private bool hasDecelerated;

    public override void EnterState()
    {
        if (!_ctx.IsAnimating("Respawn", _ctx.Animator)) _ctx.Animator.Play(_ctx.Animations[1].name);
        _ctx.Rb.gravityScale = _ctx.GravityScale * _ctx.PlayerData.GravityMult;
        _oldXInput = (int)Mathf.Sign(_ctx.XInput);
        hasDecelerated = false;  // Reset deceleration flag on entering the state
    }

    public override void UpdateState()
    {

        _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x, Mathf.Clamp(_ctx.Rb.velocity.y, -_ctx.PlayerData.ClampedVelocity, float.MaxValue));
        
        _targetSpeed = Mathf.Abs(_ctx.StoredVelocity.x) + _ctx.PlayerData.MaxSpeed + _ctx.currentSpeedModifier;
        
        if (Mathf.Abs(_ctx.Rb.velocity.x) < _targetSpeed)
        {
            Debug.Log("Acceleration");
            _ctx.Acceleration(_targetSpeed * _ctx.XInput);
        }
        else
        {
            Debug.Log("Running");
            
            _ctx.Run(_targetSpeed * Mathf.Sign(_ctx.Rb.velocity.x)); // clamp / stabilize speed
        }

        CheckSwitchState();
        if (_ctx.XInput != 0)
            _oldXInput = (int)Mathf.Sign(_ctx.XInput);
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
    
}
