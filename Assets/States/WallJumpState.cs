using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class WallJumpState : BaseState
{
    public WallJumpState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        InitialiseSubState();
        _isRootState = true;
    }

    public override void EnterState()
    {

        _ctx.Animator.Play(_ctx.Animations[3].name);

        _ctx.Rb.gravityScale = _ctx.GravityMult * _ctx.GravityScale;

        _ctx.Rb.velocity = new Vector2(0, _ctx.Rb.velocity.y);
        _ctx.AudioSource.clip = _ctx.AudioClips[0];
        _ctx.AudioSource.Play();
        _ctx.WallJumpControlCounter = 0;

        WallJump();

        
    }

    public override void UpdateState()
    {
        _ctx.WallJumpControlCounter += Time.deltaTime;

        _ctx.AccelRate = (Mathf.Abs(_ctx.TargetSpeed) > 0.01f) ? _ctx.RunAccelAmount * _ctx.AirMult : _ctx.RunDeccelAmount * _ctx.AirMult;

        CheckSwitchState();

    }

    public override void QuitState()
    {
        
    }

    public override void CheckSwitchState()
    {
        if(_ctx.YInput < 0 && _ctx.CanMove){
            SwitchState(_factory.GroundSlam());
        }else if(_ctx.IsGounded){
            SwitchState(_factory.Grounded());
        }else if(_ctx.IsWalled){
            SwitchState(_factory.Walled());
            
        }
    }

        void WallJump(){

        _ctx.BufferCounter = 0;
        _ctx.JumpPressed = false;
        
        float verticalJumpForce = Mathf.Sqrt(_ctx.WallJumpHeight * -2 * (Physics2D.gravity.y * _ctx.Rb.gravityScale));
        float timeToPeak = verticalJumpForce / Mathf.Abs(Physics2D.gravity.y * _ctx.Rb.gravityScale);
        float totalTimeInAir = -2 * timeToPeak;
        float horizontalJumpForce = _ctx.WallJumpLength / totalTimeInAir;

        float wallJumpDirection = -_ctx.PlayerTransform.localScale.x;
        _ctx.Rb.velocity = new Vector2(horizontalJumpForce * wallJumpDirection , verticalJumpForce);

        if(_ctx.PlayerTransform.localScale.x != wallJumpDirection)
        _ctx.Flip();

    }

    public override void InitialiseSubState()
    {
        if(_ctx.XInput != 0 && _ctx.CanMove){
            SetSubState(_factory.Running());
        }else{
            SetSubState(_factory.Iddle());
        }
    }
}