using System;
using System.Collections.Generic;
using System.Linq;

public class FiniteStateMachine<T, S> 
    where T : class
    where S : class
{
    public FSMState<T, S> CurrentState { get; private set; }

    private List<FSMState<T, S>> _states;

    public FiniteStateMachine() {
        _states = new List<FSMState<T, S>>();
    }

    public void AddState(FSMState<T, S> state) {
        if(HasState(state.StateId))
            throw new Exception("State " + state.StateId.ToString() + " is already defined.");

        _states.Add(state);
    }

    public void RemoveState(S stateId) {
        FSMState<T, S> state = GetState(stateId);

        if(state == null)
            throw new Exception("Could not find state " + stateId.ToString() + " for removal.");

        _states.Remove(state);
    }

    public bool HasState(S stateId) {
        return _states.Where(s => s.StateId == stateId) != null;
    }

    public FSMState<T, S> GetState(S stateId) {
        return _states.Find(s => s.StateId == stateId);
    }

    public void DoTransition(T transitionId) {
        S nextStateId = CurrentState.GetOutputState(transitionId);

        if(nextStateId == null) // this may not be okay with enums
            throw new Exception("State " + CurrentState.StateId.ToString() + " can not do transition " + transitionId.ToString());

        FSMState<T, S> nextState = GetState(nextStateId);

        if(nextState == null)
            throw new Exception("State " + nextStateId.ToString() + " has not been defined.");

        CurrentState.ExitState();

        CurrentState = nextState;

        CurrentState.EnterState();
    }
}
