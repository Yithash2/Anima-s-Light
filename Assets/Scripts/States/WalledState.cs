using UnityEngine;

public class WalledState : BaseState
{
    public WalledState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        _isRootState = true;
    }

    public override void EnterState()
    {

        if(!_ctx.IsAnimating("Respawn")) _ctx.Animator.Play(_ctx.Animations[4].name);

        _ctx.WallJumpCounter = 0;

        _ctx.Rb.gravityScale = _ctx.GravityScale * _ctx.PlayerData.GravityMult;

        _ctx.IsWallJumping = false;

        _ctx.IsDashing = false;
        _ctx.IsJumping = false;
        
    }

    public override void UpdateState()
    {

        

        _ctx.AccelerationTime = _ctx.PlayerData.AirAccelTime;

        _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x, Mathf.Clamp(_ctx.Rb.velocity.y, -_ctx.PlayerData.SlideSpeed + _ctx.PlatformVelocityY, float.MaxValue));

        if(_ctx.JumpPressed && !_ctx.IsWallJumping){
            _ctx.Rb.velocity = Vector2.zero;
            _ctx.WallJump();
        }

        CheckSwitchState();
    }

    public override void QuitState()
    {
    }

    public override void CheckSwitchState()
    {
        if(_ctx.IsSpaced()){
            SwitchState(_factory.Spaced());
        }else if(_ctx.DivePressed){
            SwitchState(_factory.GroundSlam());
        }else if(_ctx.IsGrounded()){
            SwitchState(_factory.Grounded());
            _ctx.AudioSource.clip = _ctx.AudioClips[3];
            _ctx.AudioSource.Play();
        }else if(!_ctx.IsWalled()){
            SwitchState(_factory.Falling());
        }
    }

    public override void InitialiseSubState()
    {
        if(_ctx.XInput != 0 && _ctx.CanMove()){
            SetSubState(_factory.Running());
        }else{
            SetSubState(_factory.Iddle());
        }
    }
}
