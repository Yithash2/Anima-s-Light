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

        
        UpdateSubState(_currentSubState);
        
    }

    protected void UpdateSubState(BaseState subState) {
    for (BaseState currentState = subState; currentState != null; currentState = currentState._currentSubState) {
        currentState.UpdateState();
    }
}

    protected void SwitchState(BaseState newState){
        
            
        QuitState();

        

        if(_isRootState){
            _ctx.CurrentState = newState;
            if(_currentSubState != null){
                newState.TransferSubState(_currentSubState);
            }   
        }else if(_currentSuperState != null){
            _currentSuperState.TransferSubState(newState);
        }
        
        newState.EnterState();
    }
    protected void SetSuperState(BaseState newSuperState){
        _currentSuperState = newSuperState;
    }
    protected void SetSubState(BaseState newSubState){
        if(_currentSubState != null){
            
            _currentSubState.QuitState();
            _currentSubState = newSubState;
            newSubState.SetSuperState(this);

            _currentSubState.EnterState();
        }else{
            _currentSubState = newSubState;
            newSubState.SetSuperState(this);

            _currentSubState.EnterState();
        }
        
    }

    protected void TransferSubState(BaseState newSubState){
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
    
}
