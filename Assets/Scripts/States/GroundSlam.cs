using UnityEngine;

public class GroundSlamState : BaseState
{public GroundSlamState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        _isRootState = true;
    }

    public override void EnterState()
    {
        
        if(!_ctx.IsAnimating("Respawn")) _ctx.Animator.Play(_ctx.Animations[5].name);

        

        _ctx.AudioSource.clip = _ctx.AudioClips[1];

        _ctx.AudioSource.loop = true;

        _ctx.AudioSource.Play();

        _ctx.Rb.gravityScale = _ctx.GravityScale * _ctx.PlayerData.GroundSlamMult;


    }

    public override void UpdateState(){

        _ctx.SmoothSpeed = Mathf.Lerp(_ctx.SmoothSpeed, 1, 0.0125f);

        _ctx.YCamOffcet = Mathf.Lerp(_ctx.YCamOffcet, -6, 0.0125f);


        _ctx.Rb.velocity = new Vector2(_ctx.PlatformVelocityX, Mathf.Clamp(_ctx.Rb.velocity.y, -Mathf.Abs(_ctx.PlayerData.ClampedVelocity * 2 - _ctx.PlatformVelocityY), float.MaxValue));  

        CheckSwitchState();
    }

    public override void QuitState()
    {

        _ctx.AudioSource.loop = false;

        _ctx.AudioSource.clip = _ctx.AudioClips[2];

        _ctx.AudioSource.Play();

        _ctx.SmoothSpeed = 0.125f;
    }

    public override void CheckSwitchState()
    {
        if(_ctx.IsGrounded() && _ctx.JumpPressed && _ctx.DivePressed && _ctx.XInput != 0){
            _ctx.Rb.velocity = new Vector2(Mathf.Abs(_ctx.Rb.velocity.y)*_ctx.XInput, 0);
            SwitchState(_factory.Dash());
        }else if(!_ctx.DivePressed){
            SwitchState(_factory.Falling());
            _ctx.JumpPressed = false;
        }else if(_ctx.IsGrounded()){
            SwitchState(_factory.Grounded());
        }
    }
    
    public override void InitialiseSubState(){}
}
