using UnityEngine;
using System.Collections;
public class JumpState : BaseState{
    
    public JumpState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        _stateTimer = 1;
    }

    bool hasCeilled;
    float normalpitch;
    public override void EnterState()
    {

        _ctx.HitStop(30f, 0.1f);

        _stateTimeCounter = 0;

        if(!_ctx.IsAnimating("Respawn")) _ctx.Animator.Play(_ctx.Animations[2].name);
        _ctx.AudioSource.clip = _ctx.AudioClips[0];
        normalpitch = _ctx.AudioSource.pitch;
        _ctx.AudioSource.pitch = Mathf.Pow(2, Random.Range(0, 8f)/12);
        _ctx.Rb.gravityScale = _ctx.GravityScale * _ctx.PlayerData.GravityMult;

        _ctx.AudioSource.Play();

        if(hasCeilled){
            _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x* 1.125f, -_ctx.Rb.velocity.y * 1.5f);
            hasCeilled = false;
        }else{
            _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x, 0);
        }
        
        _ctx.Rb.velocity = new Vector2(Mathf.Clamp(_ctx.Rb.velocity.x, -50, 50), _ctx.Rb.velocity.y);  


        HandleJumpStart();
        
    }

    public override void UpdateState(){

        _ctx.AccelerationTime = Mathf.Lerp(_ctx.AccelerationTime, _ctx.PlayerData.AirAccelTime, 0.2f);

        

        if(_stateTimeCounter < _stateTimer && (_ctx.JumpReleased || _ctx.Rb.velocity.y < 0) && !hasCeilled){
            _stateTimeCounter = _stateTimer + 1;
            HandleJumpEnd();
        }else{
            
            if(_ctx.IsCeilled() && !_ctx.JumpReleased){
                _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x* 1.5f, -_ctx.Rb.velocity.y* 1.5f);
                _ctx.Rb.velocity = new Vector2(Mathf.Clamp(_ctx.Rb.velocity.x, -50, 50), _ctx.Rb.velocity.y);  
                hasCeilled = true;
            }else if(_ctx.JumpReleased){
                hasCeilled = false;
            }
        }

        CheckSwitchState();
        
    }

    public override void QuitState()
    {
        _stateTimeCounter = _stateTimer + 1;
        _ctx.AudioSource.pitch = normalpitch;
    }

    public override void CheckSwitchState()
{   
    
    if (_stateTimeCounter < _stateTimer)
    {
        if (_ctx.IsGrounded() && hasCeilled && !_ctx.JumpReleased){
            SwitchState(_factory.JumpStart());
        }
        // Ensure player is pressing toward the wall and has enough x-velocity and y-velocity
        bool canWallJump = _ctx.IsWalled() &&
                           Mathf.Abs(_ctx.XInput) > 0.5f &&                  // Actively holding input toward the wall
                           Mathf.Abs(_ctx.Rb.velocity.x) > 5.5 * Mathf.Sqrt(Mathf.Abs(_ctx.Rb.velocity.y)) &&  // High enough x-velocity
                           !_ctx.JumpReleased;                            // Jump button has been released since last jump
            

        if (canWallJump)
        {
            _stateTimeCounter = _stateTimer;  // End current jump
            float wallJumpPush = Mathf.Abs(_ctx.Rb.velocity.x) - 20 > 0 ? _ctx.JumpPower + (Mathf.Abs(_ctx.Rb.velocity.x) - 15) : _ctx.JumpPower;
            _ctx.Rb.velocity = new Vector2(0, wallJumpPush);  // Apply WallJump force
            _ctx.WallJumpCombo = true;
            
            SwitchState(_factory.WallJump());  // Switch to WallJump state

            _ctx.HitStop(0, 0.2f);
        }
    }
    // Check ground condition to switch to idle or running
    else if (_ctx.IsGrounded())
    {
        if (_ctx.XInput == 0)
        {
            SwitchState(_factory.Iddle());
        }
        else
        {
            SwitchState(_factory.Running());
        }
    }
}


    public override void InitialiseSubState()
    {
        if(_ctx.XInput == 0){
            SetSubState(_factory.Rising());
        }else{
            SetSubState(_factory.JumpControlling());
        }
    }

    void HandleJumpStart(){

        _ctx.JumpReleased = false;
        _ctx.BufferCounter = 0;
        _ctx.JumpCount ++;
        _ctx.CoyoteCounter = _ctx.CoyoteTime + 1;

        _ctx.IsJumping = true;

        
        float gravity = Physics2D.gravity.y * _ctx.Rb.gravityScale;

        if(_ctx.StoredVelocity.y >= 0){
            _ctx.JumpPower = Mathf.Sqrt(_ctx.PlayerData.InitalJumpHeight * -2 * gravity)  + _ctx.StoredVelocity.y;
        }else if(_ctx.PlatformVelocityY != 0){
            float a = _ctx.StoredVelocity.y * Mathf.Sqrt(_ctx.PlayerData.InitalJumpHeight * -2 * gravity);
            _ctx.JumpPower = Mathf.Sqrt((_ctx.PlayerData.InitalJumpHeight - a) * -2 * gravity) - Mathf.Sqrt(Mathf.Abs(a) * -2 * gravity);
        }else{
            _ctx.JumpPower = Mathf.Sqrt(_ctx.PlayerData.InitalJumpHeight * -2 * gravity);
        }


            
        _ctx.Rb.velocity += new Vector2(0, _ctx.JumpPower);
        
        
        _ctx.JumpPressed = false;
    }

    void HandleJumpEnd(){
        _ctx.IsJumping = false;
        _ctx.JumpReleased = false;
        _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x,_ctx.Rb.velocity.y/3f);
    }

    





}