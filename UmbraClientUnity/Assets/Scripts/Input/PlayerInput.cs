using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {
    public delegate void PlayerMoveDelegate(Vector3 newPos);
    public event PlayerMoveDelegate OnPlayerMove = delegate { };

    private float _speed = 300.0f;

    protected void Update() {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if(h != 0)
            h = (h < 0 ? -1 : 1);

        if(v != 0)
            v = (v < 0 ? -1 : 1);

        rigidbody.velocity = new Vector3(h * _speed, v * _speed, 0);

        OnPlayerMove(gameObject.transform.position);
    }

    public void Disable() {
        enabled = false;
        rigidbody.velocity = Vector3.zero;
    }

    public void Enable() {
        enabled = true;
    }
}
