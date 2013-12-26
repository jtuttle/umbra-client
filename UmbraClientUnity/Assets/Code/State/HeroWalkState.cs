using System;

public class HeroWalkState : FSMState {
    public HeroWalkState() : 
        base(HeroState.Walk) {

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
