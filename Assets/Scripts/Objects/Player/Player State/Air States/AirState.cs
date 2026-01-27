using UnityEngine;

public class AirState : BaseState
{

    public AirState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        
        _isRootState = true;
    }

    bool _hasCeilled;

    public override void InitialiseSubState()
    {   
        if( _ctx.JumpPressed && _ctx.XInput != 0 && _ctx.CanDashInAir()){
            SetSubState(_factory.Dash()); 
        }else if(_ctx.DivePressed && _ctx.Rb.velocity.y < 0){
            SetSubState(_factory.Dive());
        }else if(_ctx.XInput != 0){
            SetSubState(_factory.Controlling()); 
        }else{
            SetSubState(_factory.Fall());
        }
        
    }

    public override void EnterState()
    {
        if (!_ctx.IsAnimating("Respawn", _ctx.Animator))  _ctx.Animator.Play(_ctx.Animations[1].name);
        _ctx._camera.IsFalling = true;
        _ctx.Rb.gravityScale = _ctx.GravityScale * _ctx.PlayerData.GravityMult;
    }

    public override void UpdateState()
    { 

        if(_ctx.IsCeilled() && !_hasCeilled  && _ctx.JumpPressed){
            _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x* 2f, -_ctx.Rb.velocity.y* 1.5f); 
            _hasCeilled = true;
        }

        _ctx.UpdateDashTimer();

        CheckSwitchState();


    }

    public override void QuitState()
    {
        _hasCeilled = false;
        _ctx._camera.IsFalling = false;
        _ctx.Rb.gravityScale = _ctx.GravityScale;
    }

    public override void CheckSwitchState()
    {
        if(_ctx.IsSpaced()){
            SwitchState(_factory.Wet());
        }else if(_ctx.IsGrounded()){
            SwitchState(_factory.Grounded());
        }else if(_ctx.IsWalled() && (_ctx.IsWallJumping || (_ctx.XInput != 0)))
        {
            _ctx.IsWallJumping = false;
            SwitchState(_factory.Walled());
        }
    }

    
}
