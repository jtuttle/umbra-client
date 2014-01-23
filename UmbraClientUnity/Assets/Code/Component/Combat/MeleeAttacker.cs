using UnityEngine;
using System.Collections;

public class MeleeAttacker : Attacker {
    public Collider AttackCollider;

    private TimeKeeper _attackTimer;

    protected void Awake() {
        AttackCollider.enabled = false;

        _attackTimer = TimeKeeper.GetTimer(0.3f, 1, "AttackTimer");
        _attackTimer.OnTimerComplete += OnAttackTimer;
        _attackTimer.transform.parent = gameObject.transform;
    }

    protected void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Hero") {
            Killable killable = other.gameObject.GetComponent<Killable>();

            if(killable == null || killable.Hittable) {
                Vector3 direction = other.transform.position - transform.position;
                other.gameObject.rigidbody.velocity = direction * 20;
            }

            if(killable != null)
                killable.TakeDamage(Damage);
        }
    }

    public void Attack() {
        AttackCollider.enabled = true;
        _attackTimer.StartTimer();
    }

    private void OnAttackTimer(TimeKeeper timer) {
        AttackCollider.enabled = false;
    }
}
