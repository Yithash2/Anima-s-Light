using UnityEngine;

public class RunState : BaseState{
    public RunState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){}

    public override void EnterState()
    {
        Debug.Log("Run");
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
        if(_ctx.xInput == 0){
            SwitchState(_factory.Iddle());
        }
    }

    public override void InitialiseSubState()
    {
        
    }

    void Run(){

        _ctx.Rb.velocity = new Vector2(_ctx.TargetSpeed * _ctx.xInput , _ctx.Rb.velocity.y);
    }
}