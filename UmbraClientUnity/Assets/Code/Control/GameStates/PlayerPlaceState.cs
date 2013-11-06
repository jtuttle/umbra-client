using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerPlaceState : ObjectPlaceState {
    public PlayerPlaceState()
        : base(new List<GameObject> { GameManager.Instance.PlayerView.gameObject }, GameStates.PlayerPlace) {

    }

    protected override void OnConfirmPress() {
        NextState = GameStates.MapWalk;

        base.OnConfirmPress();
    }

    protected override void OnCancelPress() {
        NextState = GameStates.MapDesign;

        base.OnCancelPress();
    }
}
