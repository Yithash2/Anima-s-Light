/*using System.Collections;
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
        }else if(_ctx.IsWalled() && _ctx.JumpPressed){
            SwitchState(_factory.WallJump());
        }else if(_ctx.IsWalled()){
            SwitchState(_factory.Walled());
        }
    }

        

    public override void InitialiseSubState()
    {
        
        SetSubState(_factory.Iddle());
        
    }
}*/