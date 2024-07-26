using UnityEngine;

public class FallState : BaseState{
    
    public FallState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        InitialiseSubState();
        _isRootState = true;
    }
    public override void EnterState()
    {
        _ctx.Animator.Play(_ctx.Animations[1].name);
        _ctx.Rb.gravityScale = _ctx.GravityScale * _ctx.PlayerData.GravityMult;
        _ctx.YCamOffcet = -1;

    }

    public override void UpdateState(){
        _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x, Mathf.Clamp(_ctx.Rb.velocity.y, -Mathf.Abs(_ctx.PlayerData.ClampedVelocity - _ctx.PlatformVelocityY), float.MaxValue));

        _ctx.AccelerationTime = _ctx.PlayerData.AirAccelTime;

        
        CheckSwitchState();
    }

    public override void QuitState()
    {
        _ctx.YCamOffcet = 0;


    }

    public override void CheckSwitchState()
    {
        
      if(_ctx.YInput < 0){
            SwitchState(_factory.GroundSlam());
        }else if(_ctx.IsSpaced()){
            SwitchState(_factory.Spaced());
        }else if(_ctx.IsGrounded()){
            SwitchState(_factory.Grounded());
            _ctx.AudioSource.clip = _ctx.AudioClips[3];
            _ctx.AudioSource.Play();
        }else if(_ctx.IsWalled()){
            SwitchState(_factory.Walled());
        }else if(_ctx.JumpPressed && _ctx.CanJump()){
            SwitchState(_factory.Jumping());
            
        }
    }

    public override void InitialiseSubState()
    {
        if(_ctx.XInput != 0){
            SetSubState(_factory.Running());
        }else{
            SetSubState(_factory.Iddle());
        }
    }
}