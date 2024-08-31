using UnityEngine;

public class SpaceState : BaseState
{

    public SpaceState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        InitialiseSubState();
        _isRootState = true;
    }

    public override void InitialiseSubState()
    {
        if(_ctx.XInput != 0){
            SetSubState(_factory.Running());
        }else{
            SetSubState(_factory.Iddle());
        }
    }

    public override void EnterState()
    {
        _ctx.Rb.drag = 4;
        _ctx.Bubble.SetActive(true);
    }

    public override void UpdateState()
    {  


        if(_ctx.DrowningCounter >= _ctx.DrowningTime && !_ctx.IsDead){
            _ctx.tag = "Respawn";
            _ctx.IsDead = true;
        }

        if(_ctx.JumpPressed){
            _ctx.HandleJumpStart();
        }else if(_ctx.JumpReleased){
            _ctx.HandleJumpEnd();
        }
        
        CheckSwitchState();

        if(_ctx.IsGrounded()){
            _ctx.YCamOffcet = Mathf.Lerp(_ctx.YCamOffcet, 5, 0.125f);
        }
    }

    public override void QuitState()
    {
        
    }

    public override void CheckSwitchState()
    {
        if(_ctx.DivePressed && !_ctx.IsGrounded()){
            SwitchState(_factory.GroundSlam());
        }else if(!_ctx.IsSpaced() && _ctx.IsGrounded()){
            SwitchState(_factory.Grounded());
        }else if(!_ctx.IsSpaced()){
            SwitchState(_factory.Falling());
        }
    }

    
}
