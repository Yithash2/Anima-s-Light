using UnityEngine;

public class WalledState : BaseState
{
    public WalledState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        _isRootState = true;
    }

    public override void EnterState()
    {

        _ctx.Animator.Play(_ctx.Animations[4].name);

        _ctx.MaxVelocity = _ctx.PlayerData.AirVelocity;

        _ctx.WallJumpCounter = 0;
        
    }

    public override void UpdateState()
    {
        _ctx.AccelRate = (Mathf.Abs(_ctx.TargetSpeed) > 0.01f) ? _ctx.RunAccelAmount * _ctx.PlayerData.AirMult : _ctx.RunDeccelAmount * _ctx.PlayerData.AirMult;

        _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x, Mathf.Clamp(_ctx.Rb.velocity.y, -_ctx.PlayerData.SlideSpeed, 0));

        CheckSwitchState();
    }

    public override void QuitState()
    {
        _ctx.WallJumpCounter = 0;
    }

    public override void CheckSwitchState()
    {
        if(_ctx.IsSpaced){
            SwitchState(_factory.Spaced());
        }else if(_ctx.YInput < 0){
            SwitchState(_factory.GroundSlam());
        }else if(_ctx.IsGounded){
            SwitchState(_factory.Grounded());
                    _ctx.AudioSource.clip = _ctx.AudioClips[3];
        _ctx.AudioSource.Play();
        }else if(_ctx.JumpPressed){
            SwitchState(_factory.WallJump());
        }else if(!_ctx.IsWalled){
            SwitchState(_factory.Falling());
        }
    }

    public override void InitialiseSubState()
    {
    }
}
