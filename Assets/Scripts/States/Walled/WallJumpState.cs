using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class WallJumpState : BaseState
{
    public WallJumpState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        _stateTimer = _ctx.PlayerData.WallJumpControlTime;
        _canTurn = false;

    }

    public override void EnterState()
    {
        if(!_ctx.IsAnimating("Respawn")) _ctx.Animator.Play(_ctx.Animations[3].name);
        _ctx.Rb.velocity = new Vector2(0, _ctx.Rb.velocity.y);
        _ctx.AudioSource.clip = _ctx.AudioClips[0];
        _ctx.AudioSource.Play();

        if(_ctx.Rb.velocity.y > 0){
            _stateTimeCounter = _ctx.PlayerData.WallJumpControlTime *3/4;
        }else{
            _stateTimeCounter = 0;
        }


        WallJump();

        
    }

    public override void UpdateState()
    {
        _stateTimeCounter += Time.deltaTime;

        CheckSwitchState();

    }

    public override void QuitState()
    {
        _stateTimeCounter = _stateTimer;
    }

    public override void CheckSwitchState()
    {
       if(_ctx.IsWalled() && _ctx.JumpPressed){
                SwitchState(_factory.WallJump());
        }else if(_ctx.IsWalled()){
            SwitchState(_factory.Rapelling());
        }
        
    }

    void WallJump(){

        _ctx.IsWallJumping = true;

        _ctx.JumpPressed = false;

        _ctx.BufferCounter = 0;
        _ctx.CoyoteCounter = _ctx.CoyoteTime + 1;
        
        float verticalJumpForce = Mathf.Sqrt((_ctx.PlayerData.WallJumpHeight + _ctx.StoredVelocity.y) * -2 * (Physics2D.gravity.y * _ctx.Rb.gravityScale));
        float timeToPeak = verticalJumpForce / Mathf.Abs(Physics2D.gravity.y * _ctx.Rb.gravityScale);
        float totalTimeInAir = -2 * timeToPeak;
        float horizontalJumpForce = (_ctx.PlayerData.WallJumpLength + _ctx.StoredVelocity.x) / totalTimeInAir;

        float wallJumpDirection = -_ctx.PlayerTransform.localScale.x;

        if(_ctx.Rb.velocity.y <= 0) _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x, 0);

        _ctx.Rb.velocity = new Vector2((horizontalJumpForce + Mathf.Abs(_ctx.Rb.velocity.x)) * wallJumpDirection , verticalJumpForce + Mathf.Abs(_ctx.Rb.velocity.x) + _ctx.Rb.velocity.y);

        if(_ctx.PlayerTransform.localScale.x != wallJumpDirection)
        _ctx.Flip();

    }

    public override void InitialiseSubState()
    {
        
    }
}