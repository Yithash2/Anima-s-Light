using UnityEngine;

public class WalledState : BaseState
{
    public WalledState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        _isRootState = true;
    }

    float _directionTimer = 0.3f;
    float _directionTime = 0;

    public override void EnterState()
    {

        _ctx.JumpCount = 1;

        if(!_ctx.IsAnimating("Respawn")) _ctx.Animator.Play(_ctx.Animations[4].name);
        _ctx.Rb.gravityScale = _ctx.GravityScale * _ctx.PlayerData.GravityMult;
        
        
    }

    public override void UpdateState()
    {
        _ctx.AccelerationTime = _ctx.PlayerData.AirAccelTime;

        _ctx.UpdateDashTimer();

        if (_ctx.XInput == 0) _directionTime += Time.deltaTime;
        else _directionTime = 0;

            CheckSwitchState();
    }

    public override void QuitState()
    {
        _directionTime = 0;
    }

    public override void CheckSwitchState()
    {
        if(_ctx.IsSpaced()){
            SwitchState(_factory.Wet());
        }else if(_ctx.IsGrounded()){
            SwitchState(_factory.Grounded());
            _ctx.AudioSource.clip = _ctx.AudioClips[3];
            _ctx.AudioSource.Play();
        }else if(!_ctx.IsWalled() || NoInput())
        {
            SwitchState(_factory.Normal());
        }
    }

    public override void InitialiseSubState()
    {
        if(_ctx.JumpPressed){
            SetSubState(_factory.WallJump());
        } else{
            SetSubState(_factory.Rapelling());
        }
    }

    bool NoInput()
    {
        return _directionTime >= _directionTimer;
    }
}
