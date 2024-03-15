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
        if(_ctx.XInput == 0){
            SwitchState(_factory.Iddle());
        }
    }

    public override void InitialiseSubState()
    {
        
    }

    void Run(){

        _ctx.TargetSpeed = _ctx.XInput * _ctx.MaxVelocity;
        Debug.Log(_ctx.TargetSpeed);

        float speedDif = _ctx.TargetSpeed - _ctx.Rb.velocity.x;
        Debug.Log(speedDif);

        float movement = speedDif * _ctx.AccelRate;
        Debug.Log(movement);

        _ctx.Rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }
}