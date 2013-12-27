using System;
using System.Collections.Generic;

public abstract class FSMState {
    public delegate void StateExitDelegate(Enum nextStateId);
    public event StateExitDelegate OnStateExit = delegate { };

    public Enum StateId { get; private set; }
    public Enum NextStateId { get; private set; }

    public FSMState(Enum stateId) {
        StateId = stateId;
    }

    public virtual void EnterState(FSMState prevState) {
        NextStateId = null;
    }

    public virtual void ExitState(Enum nextStateId) {
        //OnStateExit(nextStateId);

        NextStateId = nextStateId;
    }

    public virtual void Update() { }
    public virtual void Dispose() { }
}
