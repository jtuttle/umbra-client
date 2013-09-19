using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {
    public Transform Target;
    public float Speed;
	
    protected void Awake() {
        Speed = 10.0f;
    }

	protected void Update () {
        float distX = Target.position.x - gameObject.transform.position.x;
        float distY = Target.position.y - gameObject.transform.position.y;

        float angle = Mathf.Atan2(distY, distX);
        float dx = Mathf.Cos(angle) * Speed;
        float dy = Mathf.Sin(angle) * Speed;

        rigidbody.velocity = new Vector3(dx * Speed, dy * Speed, 0);
	}
}
