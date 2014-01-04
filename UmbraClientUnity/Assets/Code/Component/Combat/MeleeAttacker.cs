using UnityEngine;
using System.Collections;

public class MeleeAttacker : MonoBehaviour {
    public Collider AttackCollider;

    private TimeKeeper _attackTimer;

    protected void Awake() {
        _attackTimer = TimeKeeper.GetTimer(0.3f, 1, "AttackTimer");
        _attackTimer.OnTimerComplete += OnAttackTimer;
    }

    public void Attack() {
        /*
        Vector3 position = gameObject.transform.position;
        Vector3 direction = gameObject.transform.forward;
        float angle = Mathf.Atan2(direction.z, direction.x);

        float x = position.x + Mathf.Cos(angle) * AttackRange;
        float z = position.z + Mathf.Sin(angle) * AttackRange;

        AttackCollider.transform.position = new Vector3(x, 0, z);
        */

        AttackCollider.enabled = true;
        
        _attackTimer.StartTimer();
    }

    private void OnAttackTimer(TimeKeeper timer) {
        AttackCollider.enabled = false;
    }
}
