using UnityEngine;
using System.Collections;

public class MainState : BaseGameState {
    public MainState()
        : base(GameStates.Combat) {


    }

    public override void EnterState() {
        base.EnterState();

        // hook player input control to player
        // hook player up to camera or whatever

    }

    public override void ExitState() {
        base.ExitState();

    }

    public override void Dispose() {
        base.Dispose();

    }
}
