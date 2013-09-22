using UnityEngine;
using System.Collections;

public class MeleeAttacker : MonoBehaviour {
    public Collider AttackCollider;
    public float AttackDuration;

    private TimeKeeper _attackTimer;

	void Awake() {
        _attackTimer = TimeKeeper.GetTimer(AttackDuration, 1.0f, "AttackTimer");
        _attackTimer.OnTimerComplete += OnAttackTimerComplete;
	}

    void Destroy() {
        _attackTimer.OnTimerComplete -= OnAttackTimerComplete;
    }

    public void Attack() {
        AttackCollider.enabled = true;

        _attackTimer.ResetTimer();
        _attackTimer.StartTimer();
    }

    public void UpdateAttackColliderPosition(Vector3 position, Vector3 velocity) {
        float angle = Mathf.Atan2(velocity.y, velocity.x);

        float x = position.x + 30 * Mathf.Cos(angle);
        float y = position.y + 30 * Mathf.Sin(angle);

        AttackCollider.transform.position = new Vector3(x, y, 0);
    }

    private void OnAttackTimerComplete(TimeKeeper timer) {
        AttackCollider.enabled = false;
    }
}
