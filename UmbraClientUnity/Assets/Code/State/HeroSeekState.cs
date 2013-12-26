using System;
using UnityEngine;

public class HeroSeekState : FSMState {
    public HeroSeekState()
        : base(HeroState.Seek) {

    }

    public override void EnterState(FSMState prevState) {

    }

    public override void ExitState(Enum nextState) {

        base.ExitState(nextState);
    }

    public override void Update() {
        
    }

    public override void Dispose() {
        
    }

    private Vector3 FindDestination() {
        //GameManager.Instance.CurrentMap.

        // 1) get current room from gameobject coordinates (need to give gameobject access to state)
        // 2) choose next room to explore
        // 3) store center of chosen room
        // 4) exit state 

        return Vector3.zero;
    }
}
