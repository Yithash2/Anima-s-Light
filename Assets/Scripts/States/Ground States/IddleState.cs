using UnityEngine;

public class IddleState : BaseState
{
    public IddleState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){}

    float _currentSpeed;

    public override void EnterState()
    {
        _currentSpeed = _ctx.Rb.velocity.x;
    }

    public override void UpdateState(){
        if(Mathf.Abs(_currentSpeed) <= 10){
            _ctx.Rb.velocity = new Vector2(Mathf.Lerp(_ctx.Rb.velocity.x, 0, 0.5f), _ctx.Rb.velocity.y);
        }else{
            _ctx.Rb.velocity = new Vector2(Mathf.Lerp(_ctx.Rb.velocity.x, 0, 50/Mathf.Pow(_currentSpeed, 2)), _ctx.Rb.velocity.y);
        }
        CheckSwitchState();
    }

    public override void QuitState()
    {

    }

    public override void CheckSwitchState()
    {

        if(_ctx.JumpPressed && _ctx.CanJump()){
            SwitchState(_factory.JumpStart());
            }
        else if(_ctx.XInput != 0){
            SwitchState(_factory.Running());
        }else if(_ctx.JumpPressed && _ctx.DivePressed) SwitchState(_factory.Dash());
        
         
        
        
    }

    public override void InitialiseSubState()
    {
        
    }
}