using UnityEngine;
using System.Collections;

public class RigidBodyMover : MonoBehaviour {
    public delegate void MoveDelegate(Vector3 position, Vector3 velocity);
    public event MoveDelegate OnMove = delegate { };

    public float VelocityMax;
    public float Acceleration;

    public void Move(float h, float v) {
        if(rigidbody.velocity.magnitude < VelocityMax)
            rigidbody.velocity += new Vector3(h * Acceleration, 0, v * Acceleration);

        if(h != 0 || v != 0)
            gameObject.transform.forward = new Vector3(h, 0, v);

        OnMove(transform.position, rigidbody.velocity);
    }
}
