using UnityEngine;

public class GroundSlamState : BaseState
{public GroundSlamState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        _isRootState = true;
    }

    public override void EnterState()
    {
        _ctx.Animator.Play(_ctx.Animations[5].name);

        _ctx.Rb.velocity = new Vector2(0, 0);

        _ctx.SmoothSpeed = 0.5f;

        _ctx.AudioSource.clip = _ctx.AudioClips[1];

        _ctx.AudioSource.loop = true;

        _ctx.AudioSource.Play();

        _ctx.Rb.gravityScale = _ctx.GravityScale * _ctx.PlayerData.GroundSlamMult;

        _ctx.YCamOffcet = -3;


        _ctx.WallJumpCounter = 1000000000;

    }

    public override void UpdateState(){

        _ctx.Rb.velocity = new Vector2(0, Mathf.Clamp(_ctx.Rb.velocity.y, -40, 40));
                
        

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
        if(_ctx.IsSpaced){
            SwitchState(_factory.Spaced());
        }if(_ctx.JumpPressed){
            SwitchState(_factory.Falling());
            _ctx.JumpPressed = false;
        }
        else if(_ctx.IsGounded){
            SwitchState(_factory.Grounded());
        }
    }
    
    public override void InitialiseSubState(){}
}
