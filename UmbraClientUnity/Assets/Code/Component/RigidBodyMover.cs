using UnityEngine;
using System.Collections;

public class RigidBodyMover : MonoBehaviour {
    public delegate void MoveDelegate(Vector3 position, Vector3 velocity);
    public event MoveDelegate OnMove = delegate { };

    public float Speed;
    public float MaxSpeed;

    // TODO: this needs fixing up, it's quite wonky
    public void Move(float h, float v) {
        Vector3 targetVelocity = new Vector3(h, 0, v);
        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= Speed;

        Vector3 velocityChange = targetVelocity - rigidbody.velocity;
        velocityChange.x = Mathf.Clamp(velocityChange.x, -MaxSpeed, MaxSpeed);
        velocityChange.y = 0;
        velocityChange.z = Mathf.Clamp(velocityChange.z, -MaxSpeed, MaxSpeed);

        rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);

        OnMove(transform.position, rigidbody.velocity);
    }
}
