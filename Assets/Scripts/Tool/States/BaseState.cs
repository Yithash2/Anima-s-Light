using System;

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

    public float StateTimer {get { return _stateTimer; } protected set { _stateTimer = value; } }
    public float StateTimeCounter {get { return _stateTimeCounter; } protected set { _stateTimeCounter = value; } }

    public string CurrentStates(){
        string message = this.ToString();

        if(this._currentSubState != null){
            message += ", \n" + _currentSubState.CurrentStates();
        }

        return message;
    }

    public bool CanTurn(BaseState _currentState){
        bool canTurn = true;
        for (BaseState currentState = _currentState; currentState != null; currentState = currentState._currentSubState){
            if (!currentState._canTurn){
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

        if (_currentSubState != null){
            _currentSubState.UpdateStates();
        }
    }

    // Updates substates recursively
    protected void UpdateSubState(BaseState subState){
        if (subState != null){
            subState.UpdateState();
            subState.UpdateSubState(subState._currentSubState);
        }
    }

    // Fix to transition properly between states
    protected void SwitchState(BaseState newState)
    {
        // Block transition if any substate timer hasn't completed
        if (_currentSubState != null && !StateTimerComplete(_currentSubState))
        {
            return;  // Block transition, substate's timer hasn't finished
        }

        // Quit current state and all substates properly
        QuitAllSubStates();
        QuitState();

        // Switch context to the new state
        if (_isRootState)
        {
            _ctx.CurrentState = newState;
        }
        else if (_currentSuperState != null)
        {
            _currentSuperState.SetSubState(newState);
        }

        // Ensure the EnterState method is always called after switching
        newState.EnterState();

        // Re-initialize substate after entering new state (if needed)
        newState.InitialiseSubState();
    }

    // Quit the current substate and all nested substates
    protected void QuitAllSubStates()
    {
        if (_currentSubState != null)
        {
            _currentSubState.QuitAllSubStates();
            _currentSubState.QuitState();
            _currentSubState = null; // Clear reference to substate
        }
    }

    protected void SetSuperState(BaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(BaseState newSubState)
    {
        //_currentSubState?.QuitState();  // Quit current substate before setting a new one

        _currentSubState = newSubState;
        newSubState.SetSuperState(this);

        // Call EnterState on the new substate to ensure initialization happens
        //newSubState.EnterState();
    }

    protected bool StateTimerComplete(BaseState subState)
    {
        // Check if the substate's time counter has reached or exceeded its timer
        return subState._stateTimeCounter >= subState._stateTimer;
    }
}
