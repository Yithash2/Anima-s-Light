using UnityEngine;

public class RunState : BaseState{
    public RunState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){}


    public override void EnterState()
    {
    }

    public override void UpdateState(){

        
        

        if(Mathf.Abs(_ctx.Rb.velocity.x) > _ctx.PlayerData.MaxSpeed ){
            Run();
        }

        if(_ctx.XInput != 0){
            Acceleration();
        }

        CheckSwitchState();
    }

    public override void QuitState()
    {
        Deceleration();
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

        _ctx.Rb.velocity = new Vector2(Mathf.Sign(_ctx.Rb.velocity.x) * (_ctx.PlayerData.MaxSpeed + _ctx.BaselineVelocityX), _ctx.Rb.velocity.y);
        
    }

    void Acceleration(){
        _ctx.Rb.AddForce(new Vector2(_ctx.XInput, 0) * _ctx.Acceleration);
    }

    void Deceleration(){
        _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x/2, _ctx.Rb.velocity.y);   
    }
}