using System;
using System.Collections.Generic;

public abstract class FSMState {
    //public delegate void StateExitDelegate(Enum nextStateId);
    //public event StateExitDelegate OnStateExit = delegate { };

    public Enum StateId { get; private set; }
    public FSMTransition NextStateTransition { get; private set; }

    public FSMState(Enum stateId) {
        StateId = stateId;
    }

    public virtual void EnterState(FSMState prevState) {
        NextStateTransition = null;
    }

    public virtual void ExitState(FSMTransition nextStateTransition) {
        //OnStateExit(nextStateId);

        NextStateTransition = nextStateTransition;
    }

    public virtual void Update() { }
    public virtual void Dispose() { }
}
