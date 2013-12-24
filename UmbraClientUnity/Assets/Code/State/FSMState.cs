using System;
using System.Collections.Generic;

public abstract class FSMState<T, S>
    where T : class
    where S : class
{
    public S StateId { get; private set; }
    private Dictionary<T, S> _transitions;

    public FSMState(S stateId) {
        StateId = stateId;

        _transitions = new Dictionary<T, S>();
    }

    public void AddTransition(T transitionId, S stateId) {
        if(_transitions.ContainsKey(transitionId))
            throw new Exception("State already has transition " + transitionId.ToString() + 
                                " to state " + _transitions[transitionId].ToString());

        _transitions[transitionId] = stateId;
    }

    public void RemoveTransition(T transitionId) {
        if(!_transitions.ContainsKey(transitionId))
            throw new Exception("Transition " + transitionId.ToString() + " does not exist.");

        _transitions.Remove(transitionId);
    }

    public S GetOutputState(T transitionId) {
        return (_transitions.ContainsKey(transitionId) ? _transitions[transitionId] : null);
    }

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void Update();
    public abstract void Dispose();
}
