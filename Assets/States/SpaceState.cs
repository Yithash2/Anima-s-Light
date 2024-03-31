using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SpaceState : BaseState
{

    public SpaceState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        InitialiseSubState();
        _isRootState = true;
    }

    public override void InitialiseSubState()
    {
        if(_ctx.XInput != 0){
            SetSubState(_factory.Running());
        }else{
            SetSubState(_factory.Iddle());
        }
    }

    public override void EnterState()
    {
        _ctx.Rb.angularDrag = 4;
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    public override void QuitState()
    {
        _ctx.Rb.angularDrag = 0;
    }

    public override void CheckSwitchState()
    {
        if(_ctx.JumpPressed){
            SwitchState(_factory.Jumping());
        }else if(_ctx.YInput < 0){
            SwitchState(_factory.GroundSlam());
        }else if(!_ctx.IsSpaced && _ctx.IsGounded){
            SwitchState(_factory.Grounded());
        }else if(!_ctx.IsSpaced && _ctx.Rb.velocity.y < 0){
            SwitchState(_factory.Falling());
        }
    }

    
}
