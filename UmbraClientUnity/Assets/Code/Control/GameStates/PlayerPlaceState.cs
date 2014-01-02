using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerPlaceState : ObjectPlaceState {
    public PlayerPlaceState()
        : base(GameState.PlayerPlace) {

    }

    public override void EnterState(FSMState prevState) {
        _options = new List<GameObject> { GameManager.Instance.Player.gameObject };

        base.EnterState(prevState);
    }

    protected override void PostConfirm() {
        ExitState(new FSMTransition(GameState.MapWalk));
    }

    protected override void PostCancel() {
        ExitState(new FSMTransition(GameState.MapDesign));
    }
}
