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
        
        if(!_ctx.IsAnimating("Respawn")) _ctx.Animator.Play(_ctx.Animations[0].name);

        _ctx.Rb.gravityScale = _ctx.GravityScale;

        _ctx.IsDashing = false;
        _ctx.DashCounter = 1000;
        _ctx.IsWallJumping = false;
        _ctx.WallJumpCounter = 1000;

        _ctx.BufferCounter = 0;

    }

    public override void UpdateState(){
        _ctx.YCamOffcet = Mathf.Lerp(_ctx.YCamOffcet, 5, 0.125f);

        _ctx.AccelerationTime = Mathf.Lerp(_ctx.AccelerationTime ,_ctx.PlayerData.GroundAccelTime, 0.2f);

        if(_ctx.CanMove()){

            if(_ctx.JumpPressed && _ctx.DivePressed && _ctx.XInput != 0){
                _ctx.HandleDash();
            }else if(_ctx.JumpPressed && _ctx.CanJump()){
                _ctx.HandleJumpStart();
            }

            CheckSwitchState();
        }

        

        
    }


    public override void QuitState()
    {
    }

    public override void CheckSwitchState()
    {
        if(_ctx.IsSpaced()){
            SwitchState(_factory.Spaced());
        }else if(!_ctx.IsGrounded()){
            SwitchState(_factory.Falling());
        }
    }

    public override void InitialiseSubState()
    {
        if(_ctx.XInput != 0){
            SetSubState(_factory.Running());
        }else{
            SetSubState(_factory.Iddle());
        }
    }
}