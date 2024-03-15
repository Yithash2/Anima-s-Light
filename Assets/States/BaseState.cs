public abstract class BaseState
{
    protected bool _isRootState = false;
    protected PlayerStateMachine _ctx;
    protected StateFactory _factory;
    protected BaseState _currentSubState;
    protected BaseState _currentSuperState;

    public BaseState(PlayerStateMachine currentContext, StateFactory stateFactory){
        _ctx = currentContext;
        _factory = stateFactory;
    }

    public abstract void  EnterState();
    public abstract void UpdateState();
    public abstract void QuitState();
    public abstract void InitialiseSubState();
    public abstract void CheckSwitchState();

    public void UpdateStates(){
        UpdateState();

        if(_currentSubState != null){
            _currentSubState.UpdateState();
        }
    }
    protected void SwitchState(BaseState newState){
        QuitState();

        newState.EnterState();

        if(_isRootState){
            _ctx.CurrentState = newState;
        }else if(_currentSuperState != null){
            _currentSuperState.SetSubState(newState);
        }
        
    }
    protected void SetSuperState(BaseState newSuperState){
        _currentSuperState = newSuperState;
    }
    protected void SetSubState(BaseState newSubState){
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
    
}
