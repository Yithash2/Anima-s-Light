using UnityEngine;

public class RunState : BaseState{
    public RunState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){}

    public override void EnterState()
    {
    }

    public override void UpdateState(){

        Run();

        CheckSwitchState();
    }

    public override void QuitState()
    {
        
    }

    public override void CheckSwitchState()
    {
        if(_ctx.XInput == 0){
            SwitchState(_factory.Iddle());
        }
    }

    public override void InitialiseSubState()
    {
        
    }

    void Run(){

        _ctx.Rb.AddForce(new Vector2(_ctx.XInput, 0) * _ctx.Acceleration);

        if(Mathf.Abs(_ctx.Rb.velocity.x) > _ctx.PlayerData.MaxSpeed){
            _ctx.Rb.velocity = new Vector2(Mathf.Sign(_ctx.Rb.velocity.x) * _ctx.PlayerData.MaxSpeed + _ctx.BaselineVelocityX, _ctx.Rb.velocity.y);
        }
    }
}