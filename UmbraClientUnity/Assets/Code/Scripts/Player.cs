using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public MapViewCamera MapViewCamera;
    public MeleeAttacker PlayerAttack;

    void Awake() {
        if(MapViewCamera != null) {
            MapViewCamera.OnMoveBegin += OnCameraMoveBegin;
            MapViewCamera.OnMoveEnd += OnCameraMoveEnd;
        }

        gameObject.GetComponent<PlayerInput>().OnPlayerMove += OnPlayerMove;
        gameObject.GetComponent<PlayerInput>().OnPlayerAttack += OnPlayerAttack;
    }

    void Destroy() {
        gameObject.GetComponent<PlayerInput>().OnPlayerAttack -= OnPlayerAttack;
        gameObject.GetComponent<PlayerInput>().OnPlayerMove -= OnPlayerMove;

        if(MapViewCamera != null) {
            MapViewCamera.OnMoveEnd -= OnCameraMoveEnd;
            MapViewCamera.OnMoveBegin -= OnCameraMoveBegin;
        }
    }

    private void OnCameraMoveBegin(XY delta) {
        gameObject.GetComponent<PlayerInput>().Disable();
    }

    private void OnCameraMoveEnd(XY delta) {
        gameObject.GetComponent<PlayerInput>().Enable();
    }

    private void OnPlayerAttack() {
        PlayerAttack.Attack();
    }

    private void OnPlayerMove(Vector3 position, Vector3 velocity) {
        PlayerAttack.UpdateAttackColliderPosition(position, velocity);
    }
}
