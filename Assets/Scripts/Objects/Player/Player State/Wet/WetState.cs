using UnityEngine;

public class WetState : BaseState
{

    public WetState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        
        _isRootState = true;
    }

    private float _baseOffset;

    public override void InitialiseSubState()
    {
        if (_ctx.DivePressed && _ctx.XInput != 0 && _ctx.JumpPressed && _ctx.IsTouchingGround()) SetSubState(_factory.SeaDash());
        else if(_ctx.XInput != 0 || !_ctx.JumpReleased || _ctx.DivePressed) SetSubState(_factory.Swimming());
        else SetSubState(_factory.Sinking());

        _ctx.FxManager.WaterTint(true);
    }

    public override void EnterState()
    {
        _baseOffset = _ctx._camera.offset.y;

        _ctx.IsWallJumping = false;
        _ctx.IsJumping = false;

        _ctx.AccelerationTime = _ctx.PlayerData.AirAccelTime;
        if (!_ctx.IsAnimating("Respawn", _ctx.Animator)) _ctx.Animator.Play(_ctx.Animations[0].name);
        _ctx.Rb.drag = 4;

        _ctx.Rb.gravityScale = _ctx.GravityScale;

        _ctx.Bubble.SetActive(true);

    }

    public override void UpdateState()
    {  
        if(_ctx.DrowningCounter >= _ctx.DrowningTime && !_ctx.IsDead){
            _ctx.tag = "Respawn";
            AudioPlayer.Instance.MusicSpeed = 1;
            _ctx.IsDead = true;
        }else{
            _ctx.DrowningCounter += Time.deltaTime;
            if(_ctx.DrowningCounter < _ctx.DrowningTime*5/10){
                _ctx.Bubble.transform.localScale = Vector3.Lerp(new Vector3(3,3,0), new Vector3(1.5f,1.5f,0), (_ctx.DrowningCounter+_ctx.DrowningTime*4/10)/_ctx.DrowningTime);
            }else if(_ctx.DrowningCounter < _ctx.DrowningTime*7/10){
                _ctx.BubbleAnimator.Play("DangerZone");
                AudioPlayer.Instance.MusicSpeed = 1.3f;
            }else if(_ctx.DrowningCounter >= _ctx.DrowningCounter*9/10)
            {
                AudioPlayer.Instance.MusicSpeed = 1.8f;
                _ctx.BubbleAnimator.speed = Mathf.Lerp(1,3, (_ctx.DrowningCounter + _ctx.DrowningCounter*2/10)/_ctx.DrowningTime*8/10);
            }
        }

        
        
        if(_ctx.IsTouchingGround()) _ctx._camera.ChangeOffset(_baseOffset);
        else _ctx._camera.ChangeOffset(0);
        _ctx.JumpCount = 0;
        CheckSwitchState();
    }

    public override void QuitState()
    {
        AudioPlayer.Instance.MusicSpeed = 1;
        _ctx.FxManager.WaterTint(false);
        _ctx.DrowningCounter = 0;
        _ctx.BubbleAnimator.Play("Basic");
        _ctx.BubbleAnimator.speed = 1;
        _ctx.Bubble.SetActive(false);
        _ctx.Rb.drag = 0;
        _ctx._camera.ChangeOffset(_baseOffset);

    }

    public override void CheckSwitchState()
    {
        if(!_ctx.IsSpaced()){
            if (_ctx.IsGrounded()) SwitchState(_factory.Grounded());
            else if (_ctx.IsWalled()) SwitchState(_factory.Walled());
            else{
                _ctx.HitStop(40, 0.15f);
                SwitchState(_factory.Normal()); 
            }
        }
    }

    


}
