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

        _ctx.SmoothSpeed = 1f;

        _ctx.Rb.gravityScale = _ctx.GravityScale * _ctx.GroundSlamMult;

    }

    public override void UpdateState(){

        _ctx.Rb.velocity = new Vector2(0, Mathf.Clamp(_ctx.Rb.velocity.y, -60, 60));
                
        CheckSwitchState();
    }

    public override void QuitState()
    {
        _ctx.Rb.velocity  = new Vector2(0, 0);

        _ctx.SmoothSpeed = 0.125f;
    }

    public override void CheckSwitchState()
    {
        if(_ctx.JumpPressed){
            SwitchState(_factory.Falling());
            _ctx.JumpPressed = false;
        }
        else if(_ctx.IsGounded){
            SwitchState(_factory.Grounded());
        }
    }
    
    public override void InitialiseSubState(){}
}
