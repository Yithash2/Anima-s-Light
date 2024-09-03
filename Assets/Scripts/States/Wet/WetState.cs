using UnityEngine;

public class WetState : BaseState
{

    public WetState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        
        _isRootState = true;
    }

    public override void InitialiseSubState()
    {
        if(_ctx.XInput != 0 || _ctx.JumpPressed) SetSubState(_factory.Swimming());
        else SetSubState(_factory.Sinking());
    }

    public override void EnterState()
    {
        
        if(_currentSubState != null){
            _ctx.WaitForStateToFinish(_currentSubState);
        }else{
            InitialiseSubState();
        }
        Debug.Log("SPACE");
        _ctx.Rb.drag = 4;

        _ctx.Bubble.SetActive(true);
    }

    public override void UpdateState()
    {  
        if(_ctx.DrowningCounter >= _ctx.DrowningTime && !_ctx.IsDead){
            _ctx.tag = "Respawn";
            _ctx.IsDead = true;
        }else{
            _ctx.DrowningCounter += Time.deltaTime;
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
        _ctx.DrowningCounter = 0;
        _ctx.BubbleAnimator.Play("Basic");
        _ctx.BubbleAnimator.speed = 1;
        _ctx.Bubble.SetActive(false);
        _ctx.Rb.drag = 0;
        
    }

    public override void CheckSwitchState()
    {
        if(!_ctx.IsSpaced()){
            if(_ctx.IsGrounded()) SwitchState(_factory.Grounded());
            else SwitchState(_factory.Normal());
        }
    }

    
}
