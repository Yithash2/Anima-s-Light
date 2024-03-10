using System.Runtime.CompilerServices;
using UnityEngine;

public class GroundState : BaseState{

    public GroundState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        InitialiseSubState();
        _isRootState = true;
    }
    
    public override void EnterState()
    {
        _ctx.Rb.gravityScale = _ctx.GravityScale;

    }

    public override void UpdateState(){
        
        CheckSwitchState();

    }

    public override void QuitState()
    {
        
    }

    public override void CheckSwitchState()
    {
        if(_ctx.JumpPressed){
            SwitchState(_factory.Jumping());
        }else if(!_ctx.IsGounded && _ctx.Rb.velocity.y < 0){
            SwitchState(_factory.Falling());
        }
    }

    public override void InitialiseSubState()
    {
        if(_ctx.xInput != 0){
            SetSubState(_factory.Running());
        }else{
            SetSubState(_factory.Iddle());
        }
    }
}