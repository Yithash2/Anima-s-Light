using UnityEngine;
public class JumpState : BaseState{
    
    public JumpState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        InitialiseSubState();
        _isRootState = true;
    }
    public override void EnterState()
    {
        Debug.Log("Jump");
        HandleJumpStart();
        
    }

    public override void UpdateState(){


        if(_ctx.JumpReleased && _ctx.Rb.velocity.y >= 0) {
            HandleJumpEnd();
            
        }
        
        CheckSwitchState();
    }

    public override void QuitState()
    {
        
    }

    public override void CheckSwitchState()
    {
        if(_ctx.IsGounded){
            SwitchState(_factory.Grounded());
        }
        else if( _ctx.Rb.velocity.y <= -0.5){
            SwitchState(_factory.Falling());
        }
    }

    public override void InitialiseSubState()
    {
        if(_ctx.xInput != 0){
            SetSubState(_factory.Running());
        }else{
            SetSubState(_factory.Iddle());
        }
    }

    void HandleJumpStart(){
        _ctx.BufferCounter = 0;
        _ctx.JumpPressed = false;

        _ctx.IsJumping = true;

        _ctx.Rb.gravityScale = _ctx.GravityScale;

        float jumpForce = Mathf.Sqrt(_ctx.initalJumpHeight * -2 * (Physics2D.gravity.y * _ctx.Rb.gravityScale));
                
        _ctx.Rb.velocity += new Vector2(0, jumpForce);
    }

    void HandleJumpEnd(){
        _ctx.JumpReleased = false;

        _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x,0);
    }

}