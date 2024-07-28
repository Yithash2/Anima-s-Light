using UnityEngine;

public class GroundSlamState : BaseState
{public GroundSlamState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        _isRootState = true;
    }

    public override void EnterState()
    {
        _ctx.Animator.Play(_ctx.Animations[5].name);

        _ctx.SmoothSpeed = 0.5f;

        _ctx.AudioSource.clip = _ctx.AudioClips[1];

        _ctx.AudioSource.loop = true;

        _ctx.AudioSource.Play();

        _ctx.Rb.gravityScale = _ctx.GravityScale * _ctx.PlayerData.GroundSlamMult;

        _ctx.Rb.velocity = new Vector2(_ctx.StoredVelocity.x, _ctx.Rb.velocity.y);

        _ctx.YCamOffcet = -3;


        _ctx.WallJumpCounter = 1000000000;

    }

    public override void UpdateState(){

        _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x, Mathf.Clamp(_ctx.Rb.velocity.y, -Mathf.Abs(40 - _ctx.PlatformVelocityY), float.MaxValue));  

        CheckSwitchState();
    }

    public override void QuitState()
    {

        _ctx.YCamOffcet = 0;

        _ctx.AudioSource.loop = false;

        _ctx.AudioSource.clip = _ctx.AudioClips[2];

        _ctx.AudioSource.Play();

        _ctx.SmoothSpeed = 0.125f;
    }

    public override void CheckSwitchState()
    {
        if(_ctx.JumpPressed){
            SwitchState(_factory.Falling());
            _ctx.JumpPressed = false;
        }
        else if(_ctx.IsGrounded()){
            SwitchState(_factory.Grounded());
        }
    }
    
    public override void InitialiseSubState(){}
}
