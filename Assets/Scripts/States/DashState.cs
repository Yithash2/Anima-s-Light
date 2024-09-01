using UnityEngine;
using System.Collections;
public class DashState : BaseState{
    
    public DashState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        InitialiseSubState();
        _isRootState = true;
    }

    float horizontalJumpForce;
    public override void EnterState()
    {
        
        if(!_ctx.IsAnimating("Respawn")) _ctx.Animator.Play(_ctx.Animations[2].name);
        _ctx.AudioSource.clip = _ctx.AudioClips[0];
        _ctx.Rb.gravityScale = _ctx.GravityScale; 

        _ctx.DashCounter = 0;      

        _ctx.AudioSource.Play();

        HandleJumpStart();
        
    }

    public override void UpdateState(){

        _ctx.DashCounter += Time.deltaTime;

        if(_ctx.DashCounter > 0.4 && _ctx.XInput == _ctx.PlayerTransform.localScale.x){
            _ctx.Rb.AddForce(new Vector2(15,0));
        }

        _ctx.AccelerationTime = Mathf.Lerp(_ctx.AccelerationTime, _ctx.PlayerData.AirAccelTime, 0.2f);

        _ctx.YCamOffcet = Mathf.Lerp(_ctx.YCamOffcet, 5, 0.125f);
        
        if(_ctx.CanMove()){
            CheckSwitchState();
        }
        
    }

    public override void QuitState()
    {
        _ctx.StoredVelocity = Vector2.zero;

        _ctx.DashCounter = 1000000000;

    }

    public override void CheckSwitchState()
    {
        if(_ctx.IsSpaced()){
            SwitchState(_factory.Spaced());
        }else if(_ctx.IsWalled() && _ctx.JumpPressed){
            _ctx.JumpPressed = false;
            _ctx.Rb.velocity = new Vector2(0, horizontalJumpForce/3f);  
            SwitchState(_factory.WallJump());  
        }else if(_ctx.IsWalled()){
            SwitchState(_factory.Walled());
        }else if(_ctx.IsGrounded()){
            SwitchState(_factory.Grounded());
            _ctx.AudioSource.clip = _ctx.AudioClips[3];
            _ctx.AudioSource.Play();
        }else if(_ctx.Rb.velocity.y <= 0){
            SwitchState(_factory.Falling());
        }
    }

    public override void InitialiseSubState()
    {

    }

    void HandleJumpStart(){

        if(_ctx.StoredVelocity.y < 0){
            _ctx.StoredVelocity = Vector2.zero;
        }
        
        _ctx.BufferCounter = 0;
       
        float gravity = Physics2D.gravity.y * _ctx.Rb.gravityScale;

        float verticalJumpForce = Mathf.Sqrt((3 + _ctx.StoredVelocity.y) * -2 * gravity);
        float timeToPeak = verticalJumpForce / Mathf.Abs(gravity);
        float totalTimeInAir = 2 * timeToPeak;
        horizontalJumpForce = ((20) + _ctx.StoredVelocity.x) / totalTimeInAir;

        
        if((_ctx.Rb.velocity.y < 0 && _ctx.Rb.velocity.x == 0)){
            _ctx.Rb.velocity = Vector2.zero;
        }
        

        _ctx.ApplyForceAtEndOfFrame(new Vector2(horizontalJumpForce * _ctx.XInput , verticalJumpForce));  

    }




}