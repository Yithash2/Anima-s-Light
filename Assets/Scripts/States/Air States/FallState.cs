using UnityEngine;

public class FallState : BaseState{

    public FallState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){}

    float _targetSpeed;

    bool _decelerated;

    public override void EnterState()
    {
        if(!_ctx.IsAnimating("Respawn")) _ctx.Animator.Play(_ctx.Animations[1].name);
            _ctx.Rb.gravityScale = _ctx.GravityScale * _ctx.PlayerData.GravityMult;
    }

    public override void UpdateState(){

        CheckSwitchState();
    }

    public override void QuitState()
    {

    }

    public override void CheckSwitchState()
    {
        if(_ctx.DivePressed){
            SwitchState(_factory.Dive());
        }else if(_ctx.XInput != 0){
            SwitchState(_factory.Controlling());
        }
    }

    public override void InitialiseSubState()
    {
        
    }

}