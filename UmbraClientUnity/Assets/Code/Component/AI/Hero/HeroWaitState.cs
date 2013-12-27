using System;
using System.Collections;
using UnityEngine;

public class HeroWaitState : GameObjectState {
    public HeroWaitState(GameObject hero) : 
        base(hero, HeroState.Wait) {

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
}
