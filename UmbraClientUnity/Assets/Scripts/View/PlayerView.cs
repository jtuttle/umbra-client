using UnityEngine;
using System.Collections;

public class PlayerView : MonoBehaviour {
    public MapViewCamera MapViewCamera;
    
    private PlayerInput _playerInputRef;

    protected void Awake() {
        if(MapViewCamera != null) {
            MapViewCamera.OnMoveBegin += OnCameraMoveBegin;
            MapViewCamera.OnMoveEnd += OnCameraMoveEnd;
        }
    }

    private void OnCameraMoveBegin(XY delta) {
        gameObject.GetComponent<PlayerInput>().Disable();
    }

    private void OnCameraMoveEnd(XY delta) {
        gameObject.GetComponent<PlayerInput>().Enable();
    }
}
