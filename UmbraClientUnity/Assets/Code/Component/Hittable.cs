using UnityEngine;
using System.Collections;

public class Hittable : MonoBehaviour {
    protected void OnTriggerEnter(Collider other) {
        if(other.gameObject.name == "MeleeAttack") {
            Vector3 direction = transform.position - other.transform.position;
            gameObject.rigidbody.AddForce(direction * 2000);
        }
    }
}
