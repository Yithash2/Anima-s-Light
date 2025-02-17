using UnityEngine;

public class GroundSlamState : BaseState
{public GroundSlamState(PlayerStateMachine currentContext, StateFactory stateFactory)
    :base (currentContext, stateFactory){
        _stateTimer = 1;
    }

    float initialVelocity;

    public override void EnterState()
    {
        
        if(!_ctx.IsAnimating("Respawn")) _ctx.Animator.Play(_ctx.Animations[5].name);

        _ctx.AudioSource.clip = _ctx.AudioClips[1];

        _ctx.AudioSource.loop = true;

        _ctx.AudioSource.Play();

        _ctx.Rb.gravityScale = _ctx.GravityScale * _ctx.PlayerData.GroundSlamMult;

        if(Mathf.Abs(_ctx.Rb.velocity.x) < 40){
            initialVelocity = _ctx.Rb.velocity.x * 2f;
        }else if(Mathf.Abs(_ctx.Rb.velocity.x) > 80){
            initialVelocity = _ctx.Rb.velocity.x * 0.75f;
        }else{
            initialVelocity = _ctx.Rb.velocity.x;
        }


    }

    public override void UpdateState(){

        

        _ctx.YCamOffcet = Mathf.Lerp(_ctx.YCamOffcet, -6, 0.0125f);


        _ctx.Rb.velocity = new Vector2(_ctx.PlatformVelocityX, Mathf.Clamp(_ctx.Rb.velocity.y, -Mathf.Abs(_ctx.PlayerData.ClampedVelocity * 2 - _ctx.PlatformVelocityY), 0));  

        CheckSwitchState();
    }

    public override void QuitState()
    {
        if(Mathf.Sign(_ctx.XInput) == Mathf.Sign(initialVelocity) && _ctx.XInput != 0)_ctx.Rb.velocity = new Vector2(initialVelocity, _ctx.Rb.velocity.y/2);

        _stateTimeCounter = 0;

        _ctx.AudioSource.loop = false;
    }

    public override void CheckSwitchState()
    {   
        
        if(_ctx.IsGrounded() && _ctx.XInput != 0 && _ctx.JumpPressed){
            SwitchState(_factory.Dash());
        }else if(_ctx.IsGrounded()){
            _stateTimeCounter = 1;

            if(_ctx.XInput == 0){
                SwitchState(_factory.Iddle());
            }else{
                SwitchState(_factory.Running());
            }

            _ctx.AudioSource.loop = false;

            _ctx.AudioSource.clip = _ctx.AudioClips[2];

            _ctx.AudioSource.Play();
        }else if(!_ctx.DivePressed){
            if(_ctx.XInput == 0){
                SwitchState(_factory.Fall());
            }else{
                SwitchState(_factory.Controlling());
            }
        }

        

        
    }
    
    public override void InitialiseSubState(){}
}
