using UnityEngine;

public class FallState : BaseState{
    
    public FallState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        InitialiseSubState();
        _isRootState = true;
    }
    public override void EnterState()
    {
        if(!_ctx.IsAnimating("Respawn")) _ctx.Animator.Play(_ctx.Animations[1].name);
        _ctx.Rb.gravityScale = _ctx.GravityScale * _ctx.PlayerData.GravityMult;

        

    }

    public override void UpdateState(){
        _ctx.YCamOffcet = Mathf.Lerp(_ctx.YCamOffcet, -3, 0.5f);

        _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x, Mathf.Clamp(_ctx.Rb.velocity.y, -_ctx.PlayerData.ClampedVelocity, float.MaxValue));

        _ctx.AccelerationTime = _ctx.PlayerData.AirAccelTime;

        
        CheckSwitchState();
    }

    public override void QuitState()
    {

    }

    public override void CheckSwitchState()
    {
        
      if(_ctx.DivePressed){
            SwitchState(_factory.GroundSlam());
        }else if(_ctx.IsSpaced()){
            SwitchState(_factory.Grounded());
        }else if(_ctx.IsGrounded() && _ctx.XInput != 0 && _ctx.DivePressed){
            SwitchState(_factory.Dash());
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