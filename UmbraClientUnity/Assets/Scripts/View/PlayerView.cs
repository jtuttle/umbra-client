using UnityEngine;
using System.Collections;

public class PlayerView : MonoBehaviour {
    public MapViewCamera MapViewCamera;

    public SphereCollider AttackCollider;

    private TimeKeeper _attackTimer;

    protected void Awake() {
        if(MapViewCamera != null) {
            MapViewCamera.OnMoveBegin += OnCameraMoveBegin;
            MapViewCamera.OnMoveEnd += OnCameraMoveEnd;
        }

        _attackTimer = TimeKeeper.GetTimer(0.2f, 1.0f, "AttackTimer");
        _attackTimer.OnTimerComplete += OnAttackTimerComplete;

        gameObject.GetComponent<PlayerInput>().OnPlayerMove += OnPlayerMove;
        gameObject.GetComponent<PlayerInput>().OnPlayerAttack += OnPlayerAttack;
    }

    protected void Destroy() {
        gameObject.GetComponent<PlayerInput>().OnPlayerAttack -= OnPlayerAttack;
        gameObject.GetComponent<PlayerInput>().OnPlayerMove -= OnPlayerMove;

        _attackTimer.OnTimerComplete -= OnAttackTimerComplete;

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

    private void OnPlayerMove(Vector3 position, Vector3 velocity) {
        UpdateAttackArea(position, velocity);
    }

    private void OnPlayerAttack() {
        AttackCollider.enabled = true;

        _attackTimer.ResetTimer();
        _attackTimer.StartTimer();
    }

    private void OnAttackTimerComplete(TimeKeeper timer) {
        AttackCollider.enabled = false;
    }

    private void UpdateAttackArea(Vector3 position, Vector3 velocity) {
        float angle = Mathf.Atan2(velocity.y, velocity.x);

        float x = position.x + 30 * Mathf.Cos(angle);
        float y = position.y + 30 * Mathf.Sin(angle);

        AttackCollider.transform.position = new Vector3(x, y, 0);
    }
}
