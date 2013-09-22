using UnityEngine;
using System.Collections;

public class AttackCollider : MonoBehaviour {
    protected void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Enemy") {
            //Vector3 force = new Vector3(1000, 0, 0);

            other.gameObject.GetComponent<Follow>().React();
        }
    }
}
