using System.Runtime.CompilerServices;
using UnityEngine;

public class GroundState : BaseState{

    public GroundState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        _isRootState = true;
    }
    
    public override void EnterState()
    {
        Debug.Log(this.ToString());


        InitialiseSubState();

        if(!_ctx.IsAnimating("Respawn")) _ctx.Animator.Play(_ctx.Animations[0].name);

        _ctx.Rb.gravityScale = _ctx.GravityScale;

        

        _ctx.BufferCounter = 0;

    }

    public override void UpdateState(){
        _ctx.YCamOffcet = Mathf.Lerp(_ctx.YCamOffcet, 5, 0.125f);

        _ctx.AccelerationTime = Mathf.Lerp(_ctx.AccelerationTime ,_ctx.PlayerData.GroundAccelTime, 0.2f);

        CheckSwitchState();

        
    }


    public override void QuitState()
    {
    }

    public override void CheckSwitchState()
    {
        
        if(_ctx.IsSpaced()){
            SwitchState(_factory.Wet());
        }else if(!_ctx.IsGrounded()){
            SwitchState(_factory.Normal());
        }
    }

    public override void InitialiseSubState()
    {   
        
        if(_ctx.JumpPressed && _ctx.DivePressed && _ctx.XInput != 0){
            Debug.Log(57);
            SetSubState(_factory.Dash());
        }else if(_ctx.JumpPressed && _ctx.CanJump()){
            Debug.Log(60);
            SetSubState(_factory.JumpStart());
        }else if(_ctx.XInput != 0){
            Debug.Log(63);
            SetSubState(_factory.Running());
        }else{
            Debug.Log(66);
            SetSubState(_factory.Iddle());
        }
        
    }
}