using UnityEngine;
using System.Collections;
public class JumpState : BaseState{
    
    public JumpState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        _stateTimer = 1;
    }

    bool _hasCeilled;
    float normalpitch;

    float _releasedTime;
    public override void EnterState()
    {
        _releasedTime = 0;

        _ctx.HitStop(30f, 0.1f);

        _stateTimeCounter = 0;

        if(!_ctx.IsAnimating("Respawn", _ctx.Animator)) _ctx.Animator.Play(_ctx.Animations[2].name);
        _ctx.AudioSource.clip = _ctx.AudioClips[0];
        normalpitch = _ctx.AudioSource.pitch;
        _ctx.AudioSource.pitch = Mathf.Pow(2, Random.Range(0, 8f)/12);
        _ctx.Rb.gravityScale = _ctx.GravityScale * _ctx.PlayerData.GravityMult;

        _ctx.AudioSource.Play();

        if(_ctx.GroundType.canHeadBoost && _hasCeilled){
            _ctx.Rb.linearVelocity = new Vector2(_ctx.Rb.linearVelocity.x* 1.5f, -_ctx.Rb.linearVelocity.y * 1.5f);
            _hasCeilled = false;
            _ctx.Rb.linearVelocity = new Vector2(Mathf.Clamp(_ctx.Rb.linearVelocity.x, -50, 50),_ctx.Rb.linearVelocity.y);   
        }else{
            _ctx.Rb.linearVelocity = new Vector2(_ctx.Rb.linearVelocity.x, 0);
        }
        


        HandleJumpStart();
        
    }

    public override void UpdateState(){

        CheckSwitchState();

        if (_stateTimeCounter < _stateTimer && (_ctx.JumpReleased || _ctx.Rb.linearVelocity.y < 0) && !_hasCeilled){
            _stateTimeCounter = _stateTimer + 1;
            HandleJumpEnd();
        }else{
            
            if(_ctx.GroundType.canHeadBoost && _ctx.IsCeilled() && !_ctx.JumpReleased && !_hasCeilled){
                _ctx.Rb.linearVelocity = new Vector2(_ctx.Rb.linearVelocity.x* 1.5f, -_ctx.Rb.linearVelocity.y* 1.5f); 
                _ctx.Rb.linearVelocity = new Vector2(Mathf.Clamp(_ctx.Rb.linearVelocity.x, -50, 50),_ctx.Rb.linearVelocity.y);  
                _hasCeilled = true;
            }else if(_ctx.JumpReleased  && _hasCeilled){
                _hasCeilled = false;
            }
        }

    }

    public override void QuitState()
    {
        _stateTimeCounter = _stateTimer + 1;
        _ctx.AudioSource.pitch = normalpitch;
        _ctx.IsJumping = false;
        _hasCeilled = false;
    }

    public override void CheckSwitchState(){   
        
        if (_stateTimeCounter < _stateTimer)
        {
            if (_ctx.IsGrounded() && _hasCeilled){
                SwitchState(_factory.JumpStart());
            }

            //Debug.Log(_ctx.JumppReleasedT);
            // Ensure player is pressing toward the wall and has enough x-velocity and y-velocity
            bool canWallJump = _ctx.IsWalled() &&
                            _ctx.DivePressed;                            // Jump button has been released since last jump
                

            if (canWallJump)
            {
                _stateTimeCounter = _stateTimer;  // End current jump
                float adhesion = _ctx.GroundType.adhesion > 1 ? 1f : _ctx.GroundType.adhesion;
                float wallJumpPush = Mathf.Abs(_ctx.Rb.linearVelocity.x) - _ctx.PlayerData.MaxSpeed > 0 ? _ctx.JumpPower + (Mathf.Abs(_ctx.Rb.linearVelocity.x) - _ctx.PlayerData.MaxSpeed - _ctx.GroundType.maxSpeedAddifier) : _ctx.JumpPower;
                _ctx.Rb.linearVelocity = new Vector2(0, wallJumpPush * adhesion);  // Apply WallJump force
                _ctx.StartWallJumpComboCoroutine(1);
                _ctx.WallJumpCombo = true;

                _ctx._camera.ResetPostion();
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

        _ctx.BufferCounter = 0;
        _ctx.JumpCount ++;
        _ctx.CoyoteCounter = _ctx.CoyoteTime + 1;

        _ctx.IsJumping = true;

        
        float gravity = Physics2D.gravity.y * _ctx.Rb.gravityScale;

        
        _ctx.JumpPower = Mathf.Sqrt(_ctx.PlayerData.InitalJumpHeight * -2 * gravity);

        _ctx.Rb.linearVelocity += new Vector2(0, _ctx.JumpPower);

        //Un saut complet du joueur fait 14.1550993002wu de distance , et 6wu de haut sans élant
        
        
        _ctx.JumpPressed = false;
    }

    void HandleJumpEnd(){
        _ctx.IsJumping = false;
        _ctx.Rb.linearVelocity = new Vector2(_ctx.Rb.linearVelocity.x,_ctx.Rb.linearVelocity.y/1.5f);
    }

    





}