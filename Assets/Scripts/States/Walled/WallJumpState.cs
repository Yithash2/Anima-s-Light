using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class WallJumpState : BaseState
{
    public WallJumpState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        
    }

    public override void EnterState()
    {
        if(!_ctx.IsAnimating("Respawn")) _ctx.Animator.Play(_ctx.Animations[3].name);

        Debug.Log("FDDD");
        _ctx.Rb.velocity = new Vector2(0, _ctx.Rb.velocity.y);
        _ctx.AudioSource.clip = _ctx.AudioClips[0];
        _ctx.AudioSource.Play();

        _stateFinished = false;

        if(_ctx.Rb.velocity.y > 0){
            _ctx.WallJumpCounter = _ctx.PlayerData.WallJumpControlTime/4;
        }else{
            _ctx.WallJumpCounter = 0;
        }

        WallJump();

        
    }

    public override void UpdateState()
    {
        if(_ctx.WallJumpCounter < _ctx.PlayerData.WallJumpControlTime){
            _ctx.WallJumpCounter += Time.deltaTime;
        }else{
            _stateFinished = true;
        }

        CheckSwitchState();

    }

    public override void QuitState()
    {
        _ctx.JumpPressed = false;
        _ctx.IsWallJumping = false;
        _stateFinished = true;
    }

    public override void CheckSwitchState()
    {
        if(_ctx.IsWalled() && _ctx.JumpPressed){
            SwitchState(_factory.WallJump());
        }else if(_stateFinished){
            if(_ctx.IsWalled()){
                SwitchState(_factory.Rapelling());
            }else{
                SwitchState(_factory.Fall());
            }
            
        }
        
    }

    void WallJump(){

        _ctx.IsWallJumping = true;

        _ctx.BufferCounter = 0;
        _ctx.CoyoteCounter = _ctx.CoyoteTime + 1;
        
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
        
    }
}