using UnityEngine;
using System.Collections;

public class ContactAttacker : Attacker {
    protected void OnCollisionEnter(Collision other) {
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
}
