using System;
using UnityEngine;

public class HeroWalkState : FSMState {
    private Vector3 _destination;

    public HeroWalkState() : 
        base(HeroState.Walk) {

    }

    public override void EnterState(FSMState prevState) {
        if((HeroState)prevState.StateId == HeroState.Seek)
            _destination = (prevState as HeroSeekState).Destination;

        Debug.Log("destination is " + _destination);
    }

    public override void ExitState(Enum nextState) {

        base.ExitState(nextState);
    }

    public override void Update() {
        
    }

    public override void Dispose() {
        
    }
}
