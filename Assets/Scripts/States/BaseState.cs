public abstract class BaseState
{
    protected bool _isRootState = false;
    protected PlayerStateMachine _ctx;
    protected StateFactory _factory;
    protected BaseState _currentSubState;
    protected BaseState _currentSuperState;
    protected bool _stateFinished = true;

    public bool StateFinished {get { return _stateFinished; }}

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

        

        if(_isRootState){
            _ctx.CurrentState = newState;
            if(_currentSubState != null){
                newState.SetSubState(_currentSubState);
            }   
        }else if(_currentSuperState != null){
            _currentSuperState.SetSubState(newState);
        }
        
        newState.EnterState();
    }
    protected void SetSuperState(BaseState newSuperState){
        _currentSuperState = newSuperState;
    }
    protected void SetSubState(BaseState newSubState){

        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
        
    }
    
}
