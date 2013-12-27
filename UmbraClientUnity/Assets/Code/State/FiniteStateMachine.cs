using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FiniteStateMachine {
    public FSMState CurrentState { get; private set; }

    private List<FSMState> _states;

    public FiniteStateMachine() {
        _states = new List<FSMState>();
    }

    public void AddState(FSMState state) {
        if(HasState(state.StateId))
            throw new Exception("State " + state.StateId.ToString() + " is already defined.");

        _states.Add(state);
    }

    public void RemoveState(Enum stateId) {
        FSMState state = GetState(stateId);

        if(state == null)
            throw new Exception("Could not find state " + stateId.ToString() + " for removal.");

        _states.Remove(state);
    }

    public bool HasState(Enum stateId) {
        return _states.Any(s => s.StateId == stateId);
    }

    public FSMState GetState(Enum stateId) {
        return _states.Find(s => s.StateId.ToString() == stateId.ToString());
    }

    public void ChangeState(Enum stateId) {
        Debug.Log("changing state to: " + stateId);

        FSMState nextState = GetState(stateId);

        if(nextState == null)
            throw new Exception("State " + stateId.ToString() + " has not been defined.");

        FSMState prevState = CurrentState;
        CurrentState = nextState;

        CurrentState.OnStateExit += OnStateExit;
        CurrentState.EnterState(prevState);
    }

    public void Update() {
        CurrentState.Update();

        if(CurrentState.NextStateId != null)
            ChangeState(CurrentState.NextStateId);
    }

    private void OnStateExit(Enum nextStateId) {
        ChangeState(nextStateId);
    }
}
