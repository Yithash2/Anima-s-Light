using UnityEngine;
using System.Collections;
public class JumpEndState : BaseState{
    
    public JumpEndState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        
    }

    public override void EnterState()
    { 
        HandleJumpEnd();
        
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
        }else if(_ctx.XInput == 0){
            SwitchState(_factory.Fall());
        }else{
            SwitchState(_factory.Controlling());
        }
    }

    public override void InitialiseSubState()
    {

    }

    void HandleJumpEnd(){
        _ctx.IsJumping = false;
        _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x,_ctx.Rb.velocity.y/3);
    }




}