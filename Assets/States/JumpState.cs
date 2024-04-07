using UnityEngine;
public class JumpState : BaseState{
    
    public JumpState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        InitialiseSubState();
        _isRootState = true;
    }
    public override void EnterState()
    {
        _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x, 0);

        _ctx.Animator.Play(_ctx.Animations[2].name);
        _ctx.AudioSource.clip = _ctx.AudioClips[0];
        _ctx.Rb.gravityScale = _ctx.GravityScale;

        _ctx.YCamOffcet = -1;

        _ctx.AudioSource.Play();

        HandleJumpStart();
        
    }

    public override void UpdateState(){

        if(_ctx.JumpReleased) {
            HandleJumpEnd();
        }
        
        CheckSwitchState();
    }

    public override void QuitState()
    {

    }

    public override void CheckSwitchState()
    {
        if(_ctx.IsSpaced()){
            SwitchState(_factory.Spaced());
        }else if(_ctx.YInput < 0){
            SwitchState(_factory.GroundSlam());
        }else if(_ctx.IsGrounded()){
            SwitchState(_factory.Grounded());
            _ctx.AudioSource.clip = _ctx.AudioClips[3];
        _ctx.AudioSource.Play();
        }else if(_ctx.IsWalled()){
            SwitchState(_factory.Walled());
        }else if(_ctx.Rb.velocity.y <= -0.5){
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

    void HandleJumpStart(){

        _ctx.BufferCounter = 0;
        _ctx.JumpPressed = false;
        _ctx.JumpCount ++;
        _ctx.CoyoteCounter = _ctx.CoyoteTime + 1;

        _ctx.IsJumping = true;

        float jumpForce = Mathf.Sqrt(_ctx.PlayerData.InitalJumpHeight * -2 * (Physics2D.gravity.y * _ctx.Rb.gravityScale));
                
        if(_ctx.IsSpaced()){
            _ctx.Rb.velocity += new Vector2(_ctx.Rb.velocity.x, jumpForce + _ctx.BaselineVelocityY);
        }else{
            _ctx.Rb.velocity += new Vector2(0, jumpForce + _ctx.BaselineVelocityY);
        }
        
    }

    void HandleJumpEnd(){
        _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x,_ctx.Rb.velocity.y/3);
        _ctx.IsJumping = true;
    }

}