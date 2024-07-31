using UnityEngine;

public class IddleState : BaseState
{
    public IddleState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){}

    

    public override void EnterState()
    {
        if(!_ctx.IsJumping && _ctx.IsOnMovingPlatform){
            _ctx.Rb.velocity = _ctx.PlatformVelocityY <= 0 ? new Vector2(_ctx.PlatformVelocityX, _ctx.PlatformVelocityY * _ctx.GravityScale) : new Vector2(_ctx.PlatformVelocityX, Mathf.Clamp(_ctx.Rb.velocity.y, float.MinValue, 0));

        }

    }

    public override void UpdateState(){

        if(!_ctx.IsJumping && _ctx.IsOnMovingPlatform){
            _ctx.Rb.velocity = _ctx.PlatformVelocityY <= 0 ? new Vector2(_ctx.PlatformVelocityX, _ctx.PlatformVelocityY * _ctx.GravityScale) : new Vector2(_ctx.PlatformVelocityX, Mathf.Clamp(_ctx.Rb.velocity.y, float.MinValue, 0));
        }


        CheckSwitchState();
    }

    public override void QuitState()
    {
        if(!_ctx.IsJumping && _ctx.IsOnMovingPlatform){
            _ctx.Rb.velocity = _ctx.PlatformVelocityY <= 0 ? new Vector2(_ctx.PlatformVelocityX, _ctx.PlatformVelocityY * _ctx.GravityScale) : new Vector2(_ctx.PlatformVelocityX, Mathf.Clamp(_ctx.Rb.velocity.y, float.MinValue, 0));

        }
    }

    public override void CheckSwitchState()
    {
        if(_ctx.XInput != 0 && _ctx.CanMove()){
            SwitchState(_factory.Running());
        }
    }

    public override void InitialiseSubState()
    {
        
    }
}