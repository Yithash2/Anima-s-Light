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

        if(!_ctx.IsAnimating("Respawn")) _ctx.Animator.Play(_ctx.Animations[3].name);

        _ctx.Rb.gravityScale = _ctx.PlayerData.GravityMult * _ctx.GravityScale;

        _ctx.Rb.velocity = new Vector2(0, _ctx.Rb.velocity.y);
        _ctx.AudioSource.clip = _ctx.AudioClips[0];
        _ctx.AudioSource.Play();

        if(_ctx.Rb.velocity.y > 0){
            _ctx.WallJumpCounter = _ctx.PlayerData.WallJumpControlTime*3/4;
        }else{
            _ctx.WallJumpCounter = 0;
        }

        WallJump();

        
    }

    public override void UpdateState()
    {
        _ctx.WallJumpCounter += Time.deltaTime;

        _ctx.AccelerationTime = _ctx.PlayerData.AirAccelTime;

        CheckSwitchState();

    }

    public override void QuitState()
    {
        _ctx.JumpPressed = false;
        _ctx.WallJumpCounter = 100000000;
    }

    public override void CheckSwitchState()
    {
        if(_ctx.IsSpaced()){
            SwitchState(_factory.Spaced());
        }else if(_ctx.Rb.velocity.y < _ctx.StoredVelocity.y && _ctx.CanMove()){
            SwitchState(_factory.Falling());
        }else if(_ctx.DivePressed && _ctx.CanMove()){
            SwitchState(_factory.GroundSlam());
        }else if(_ctx.IsGrounded()){
            SwitchState(_factory.Grounded());
            _ctx.AudioSource.clip = _ctx.AudioClips[3];
            _ctx.AudioSource.Play();
        }else if(_ctx.IsWalled()){
            SwitchState(_factory.Walled());
        }
    }

        void WallJump(){

        _ctx.BufferCounter = 0;
        
        float verticalJumpForce = Mathf.Sqrt((_ctx.PlayerData.WallJumpHeight + _ctx.StoredVelocity.y) * -2 * (Physics2D.gravity.y * _ctx.Rb.gravityScale));
        float timeToPeak = verticalJumpForce / Mathf.Abs(Physics2D.gravity.y * _ctx.Rb.gravityScale);
        float totalTimeInAir = -2 * timeToPeak;
        float horizontalJumpForce = (_ctx.PlayerData.WallJumpLength + _ctx.StoredVelocity.x) / totalTimeInAir;

        float wallJumpDirection = -_ctx.PlayerTransform.localScale.x;

        if(_ctx.Rb.velocity.y <= 0) _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x, 0);

        _ctx.Rb.velocity += new Vector2(horizontalJumpForce * wallJumpDirection , verticalJumpForce + Mathf.Abs(_ctx.Rb.velocity.x));

        if(_ctx.PlayerTransform.localScale.x != wallJumpDirection)
        _ctx.Flip();

    }

    public override void InitialiseSubState()
    {
        
        SetSubState(_factory.Iddle());
        
    }
}