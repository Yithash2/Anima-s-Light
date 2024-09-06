public abstract class BaseState
{
    protected bool _isRootState = false;
    protected PlayerStateMachine _ctx;
    protected StateFactory _factory;
    protected BaseState _currentSubState;
    protected BaseState _currentSuperState;
    
    protected float _stateTimer = 0;
    protected float _stateTimeCounter = 0;

    protected bool _canTurn = true;

    public float StateTimer {get { return _stateTimer;}protected set { _stateTimer = value; }}
    public float StateTimeCounter {get { return _stateTimeCounter;}protected set { _stateTimeCounter = value; }}

    public bool CanTurn(BaseState _currentState){
        bool canTurn = true;
        for (BaseState currentState = _currentState; currentState != null; currentState = currentState._currentSubState){
            if(!currentState._canTurn){
                canTurn = false;
                break;
            }
        }
        return canTurn;
    }

    public BaseState(PlayerStateMachine currentContext, StateFactory stateFactory){
        _ctx = currentContext;
        _factory = stateFactory;
    }

    public abstract void EnterState();
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
        
        if(_currentSubState != null){
            if(_currentSubState._stateTimeCounter >= _currentSubState._stateTimer){
                QuitState();

                if(_isRootState){
                    _ctx.CurrentState = newState;
                }else if(_currentSuperState != null){
                    _currentSuperState.SetSubState(newState);
                }
                
                newState.EnterState();
            }
        }else{
            QuitState();

            if(_isRootState){
                _ctx.CurrentState = newState;
            }else if(_currentSuperState != null){
                _currentSuperState.SetSubState(newState);
            }
            
            newState.EnterState();
        }
    }
    protected void SetSuperState(BaseState newSuperState){
        _currentSuperState = newSuperState;
    }
    protected void SetSubState(BaseState newSubState){
        if(_currentSubState != null){
            
            _currentSubState.QuitState();
            _currentSubState = newSubState;
            newSubState.SetSuperState(this);

            //_currentSubState.EnterState();
        }else{
            _currentSubState = newSubState;
            newSubState.SetSuperState(this);

            //_currentSubState.EnterState();
        }
        
    }
    
}
