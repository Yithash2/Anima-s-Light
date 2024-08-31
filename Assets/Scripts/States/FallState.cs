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

        _ctx.AccelerationTime = Mathf.Lerp(_ctx.AccelerationTime, _ctx.PlayerData.AirAccelTime, 0.2f);

        if(_ctx.CanMove()){
            if(_ctx.IsWalled() && _ctx.IsJumping && _ctx.XInput == 0 && Mathf.Sqrt(5*Mathf.Abs(_ctx.Rb.velocity.y)) < Mathf.Abs(_ctx.Rb.velocity.x)){
                _ctx.Rb.velocity = new Vector2(0,_ctx.JumpForce);
                _ctx.IsJumping = false;
                _ctx.WallJump();
            }

            if(_ctx.JumpReleased && _ctx.Rb.velocity.y > 0 && _ctx.IsJumping) {
                _ctx.HandleJumpEnd();
            }
            
            if(_ctx.JumpPressed && _ctx.DivePressed && _ctx.XInput != 0 && _ctx.IsGrounded()){
                _ctx.HandleDash();
            }
        }

        if(_ctx.IsDashing && _ctx.IsWalled() && _ctx.JumpPressed){
            _ctx.IsDashing = false;
            _ctx.Rb.velocity = new Vector2(0, _ctx.HorizontalDashForce/3);
            _ctx.WallJump();
        } 
        
        CheckSwitchState();
    }

    public override void QuitState()
    {

    }

    public override void CheckSwitchState()
    {
        if(_ctx.CanMove()){
            if(_ctx.DivePressed && (!_ctx.IsDashing || !_ctx.IsJumping)){
                SwitchState(_factory.GroundSlam());
            }else if(_ctx.IsSpaced()){
                SwitchState(_factory.Spaced());
            }
        }
            
            if(_ctx.IsWalled()){
                SwitchState(_factory.Walled());
            }else if(_ctx.IsGrounded()){
                SwitchState(_factory.Grounded());
                _ctx.AudioSource.clip = _ctx.AudioClips[3];
                _ctx.AudioSource.Play();
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