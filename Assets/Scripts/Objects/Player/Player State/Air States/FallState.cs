using UnityEngine;

public class FallState : BaseState{

    public FallState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){}

    public override void EnterState()
    {
        if(!_ctx.IsAnimating("Respawn")) _ctx.Animator.Play(_ctx.Animations[1].name);
            _ctx.Rb.gravityScale = _ctx.GravityScale * _ctx.PlayerData.GravityMult;
        _ctx._camera.ResetPostion();
    }

    public override void UpdateState(){
        _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x, Mathf.Clamp(_ctx.Rb.velocity.y, -_ctx.PlayerData.ClampedVelocity, float.MaxValue));

        _ctx.Rb.velocity = new Vector2(Mathf.Lerp(_ctx.Rb.velocity.x, 0, 0.05f), _ctx.Rb.velocity.y);

        CheckSwitchState();
    }

    public override void QuitState()
    {

    }

    public override void CheckSwitchState()
    {
         if( _ctx.JumpPressed && _ctx.XInput != 0 && _ctx.CanDashInAir()){
            SwitchState(_factory.Dash()); 
            return;
        }else if(_ctx.DivePressed){
            SwitchState(_factory.Dive());
        }else if(_ctx.XInput != 0){
            SwitchState(_factory.Controlling());
        }

        
    }

    public override void InitialiseSubState()
    {
        
    }

}