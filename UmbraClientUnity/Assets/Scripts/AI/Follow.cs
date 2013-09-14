using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {
    public Transform Target;
    public float Speed;
	
    protected void Awake() {
        Speed = 0.5f;
    }

	protected void Update () {
        float distX = Target.position.x - gameObject.transform.position.x;
        float distY = Target.position.y - gameObject.transform.position.y;

        float angle = Mathf.Atan2(distY, distX);
        float dx = Mathf.Cos(angle) * Speed;
        float dy = Mathf.Sin(angle) * Speed;

        transform.position = transform.position + new Vector3(dx, dy, 0);
	}
}
