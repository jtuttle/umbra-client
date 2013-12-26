﻿using System;
using System.Collections.Generic;
using System.Linq;

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
        return _states.Where(s => s.StateId == stateId) != null;
    }

    public FSMState GetState(Enum stateId) {
        return _states.Find(s => s.StateId == stateId);
    }

    public void ChangeState(Enum stateId) {
        FSMState nextState = GetState(stateId);

        if(nextState == null)
            throw new Exception("State " + stateId.ToString() + " has not been defined.");

        FSMState prevState = CurrentState;
        CurrentState = nextState;

        CurrentState.OnStateExit += OnStateExit;
        CurrentState.EnterState(prevState);
    }

    private void OnStateExit(Enum nextStateId) {
        ChangeState(nextStateId);
    }
}