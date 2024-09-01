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
        }else{
            if(_ctx.DrowningCounter < _ctx.DrowningTime*6/10){
                Debug.Log((_ctx.DrowningCounter+_ctx.DrowningTime*4/10)/_ctx.DrowningTime);
                _ctx.Bubble.transform.localScale = Vector3.Lerp(new Vector3(3,3,0), new Vector3(1.5f,1.5f,0), (_ctx.DrowningCounter+_ctx.DrowningTime*4/10)/_ctx.DrowningTime);
            }else if(_ctx.DrowningCounter < _ctx.DrowningTime*8/10){
                _ctx.BubbleAnimator.Play("DangerZone");
            }else if(_ctx.DrowningCounter >= _ctx.DrowningCounter*8/10){
                _ctx.BubbleAnimator.speed = Mathf.Lerp(1,3, (_ctx.DrowningCounter + _ctx.DrowningCounter*2/10)/_ctx.DrowningTime*8/10);
            }
        }

        _ctx.JumpCount = 0;
        CheckSwitchState();
    }

    public override void QuitState()
    {
        

    }

    public override void CheckSwitchState()
    {
        if(_ctx.JumpPressed && _ctx.CanJump()){
            SwitchState(_factory.Jumping());
        }else if(_ctx.DivePressed && !_ctx.IsGrounded()){
            SwitchState(_factory.GroundSlam());
        }else if(!_ctx.IsSpaced() && _ctx.IsGrounded()){
            SwitchState(_factory.Grounded());
        }else if(!_ctx.IsSpaced() && _ctx.Rb.velocity.y < 0){
            SwitchState(_factory.Falling());
        }
    }

    
}
