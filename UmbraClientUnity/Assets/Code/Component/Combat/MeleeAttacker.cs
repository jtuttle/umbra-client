using UnityEngine;
using System.Collections;

public class MeleeAttacker : MonoBehaviour {
    public int Damage;
    public Collider AttackCollider;

    private TimeKeeper _attackTimer;

    protected void Awake() {
        Damage = 1;

        _attackTimer = TimeKeeper.GetTimer(0.3f, 1, "AttackTimer");
        _attackTimer.transform.parent = gameObject.transform;
        _attackTimer.OnTimerComplete += OnAttackTimer;
    }

    protected void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Enemy") {
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
