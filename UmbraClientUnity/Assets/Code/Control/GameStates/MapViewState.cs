using UnityEngine;
using System.Collections;

public class MapViewState : BaseGameState {
    public MapViewState() 
        : base(GameStates.MapView) {

            UnityUtils.LoadResource<GameObject>("Prefabs/MapOverlay", true);
    }

    public override void EnterState() {
        base.EnterState();

    }

    public override void ExitState() {

        base.ExitState();
    }

    public override void Dispose() {
        base.Dispose();

    }
}
