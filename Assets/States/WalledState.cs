using UnityEngine;

public class WalledState : BaseState
{
    public WalledState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        InitialiseSubState();
        _isRootState = true;
    }

    public override void EnterState()
    {

        _ctx.Animator.Play(_ctx.Animations[4].name);
        

        Debug.Log("IS WALLED");
    }

    public override void UpdateState()
    {
        _ctx.AccelRate = (Mathf.Abs(_ctx.TargetSpeed) > 0.01f) ? _ctx.RunAccelAmount * _ctx.AirMult : _ctx.RunDeccelAmount * _ctx.AirMult;

        _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x, Mathf.Clamp(_ctx.Rb.velocity.y, -_ctx.SlideSpeed, 0));

        CheckSwitchState();
    }

    public override void QuitState()
    {

    }

    public override void CheckSwitchState()
    {
        if(_ctx.YInput < 0){
            SwitchState(_factory.GroundSlam());
        }else if(_ctx.IsGounded){
            SwitchState(_factory.Grounded());
        }else if(!_ctx.isWalled){
            SwitchState(_factory.Falling());
        }else if(_ctx.JumpPressed){
            SwitchState(_factory.WallJump());
        }
    }

    public override void InitialiseSubState()
    {
        if(_ctx.XInput != 0){
            SetSubState(_factory.Running());
        }
    }
}
