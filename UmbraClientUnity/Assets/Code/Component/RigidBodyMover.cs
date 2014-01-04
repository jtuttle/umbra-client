using UnityEngine;
using System.Collections;

public class RigidBodyMover : MonoBehaviour {
    public delegate void MoveDelegate(Vector3 position, Vector3 velocity);
    public event MoveDelegate OnMove = delegate { };

    public float Speed;

    public void Move(float h, float v) {
        Vector3 moveDirection = new Vector3(h * Speed, 0, v * Speed);

        rigidbody.velocity = new Vector3(moveDirection.x, 0, moveDirection.z);
        rigidbody.AddForce(Vector3.up * -10);
        
        if(moveDirection != Vector3.zero)
            gameObject.transform.forward = moveDirection;

        OnMove(transform.position, rigidbody.velocity);
    }
}
