using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {
    public Transform Target;
    
    private float _velocityMax = 10.0f;
    private float _acceleration = 0.1f;

    private float Speed;

    protected void Awake() {
        Speed = 10.0f;
    }

	protected void Update () {
        float angle = AngleToTarget();
        float dx = Mathf.Cos(angle) * Speed;
        float dy = Mathf.Sin(angle) * Speed;

        rigidbody.velocity = new Vector3(dx * Speed, dy * Speed, 0);
	}

    private float AngleToTarget() {
        float distX = Target.position.x - gameObject.transform.position.x;
        float distY = Target.position.y - gameObject.transform.position.y;
        return Mathf.Atan2(distY, distX);
    }   
}
