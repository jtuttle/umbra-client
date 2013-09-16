using UnityEngine;
using System.Collections;

public class PlayerView : MonoBehaviour {
    public MapViewCamera MapViewCamera;
    
    private PlayerInput _playerInputRef;

    protected void Awake() {
        _playerInputRef = gameObject.GetComponent<PlayerInput>();

        MapViewCamera.OnMoveBegin += OnCameraMoveBegin;
        MapViewCamera.OnMoveEnd += OnCameraMoveEnd;
    }

    private void OnCameraMoveBegin(XY delta) {
        _playerInputRef.enabled = false;
    }

    private void OnCameraMoveEnd(XY delta) {
        _playerInputRef.enabled = true;
    }
}
