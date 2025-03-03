using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

public class WallJumpState : BaseState
{
    public WallJumpState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        _stateTimer = _ctx.PlayerData.WallJumpControlTime;
        _canTurn = false;
    }

    bool _hasCeilled;
    public override void EnterState()
    {
        if(!_ctx.IsAnimating("Respawn")) _ctx.Animator.Play(_ctx.Animations[3].name);
        _ctx.AudioSource.clip = _ctx.AudioClips[0];
        _ctx.AudioSource.Play();

        if(!_ctx.HasWallJumpCombo && Mathf.Abs(_ctx.Rb.velocity.y) > 1)_ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x, _ctx.Rb.velocity.y/Mathf.Log10(Mathf.Abs(_ctx.Rb.velocity.y*10)));
        else if(_ctx.HasWallJumpCombo)_ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x * 1.5f, _ctx.Rb.velocity.y);
        
        //Debug.Log("WALLJUMP");
        if(_ctx.HasWallJumpCombo){
            _stateTimeCounter = _ctx.PlayerData.WallJumpControlTime *3/4;
        }else{
            _stateTimeCounter = 0;
        }

        _ctx.WallJumpCombo = false;


        WallJump();

    }

    public override void UpdateState()
    {
        _stateTimeCounter += Time.deltaTime;

        if(_ctx.IsCeilled() && !_hasCeilled && _ctx.JumpPressed){
            _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x* 2f, -_ctx.Rb.velocity.y* 1.5f); 
            _stateTimeCounter = _stateTimer;
            _hasCeilled = true;
        }

        if (_ctx.IsSpaced() || _ctx.IsTouchingGround())
        {
            _stateTimeCounter = _stateTimer;
        }

        CheckSwitchState();

    }

    public override void QuitState()
    {
        _stateTimeCounter = _stateTimer;
        _hasCeilled = false;
    }

    public override void CheckSwitchState()
    {   

        if(_ctx.IsWalled() && _ctx.JumpPressed){
            SwitchState(_factory.WallJump());
        }else if(_ctx.IsWalled()){
            SwitchState(_factory.Rapelling());
        }

        if(_currentSuperState is AirState && _stateTimeCounter >= _stateTimer){
            _currentSuperState.InitialiseSubState();
            return;
        }

        if(_currentSuperState is GroundState && _stateTimeCounter >= _stateTimer)
        {
            _currentSuperState.InitialiseSubState();
            return;
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

    // Update the rigidbody's velocity for wall jumping
        _ctx.Rb.velocity = new Vector2((horizontalJumpForce - Mathf.Abs(_ctx.Rb.velocity.x/10)) * wallJumpDirection, verticalJumpForce + _ctx.Rb.velocity.y + Mathf.Abs(_ctx.Rb.velocity.x/10));

        if(_ctx.PlayerTransform.localScale.x != wallJumpDirection)
        _ctx.Flip();

    }

    public override void InitialiseSubState()
    {
        
    }
}