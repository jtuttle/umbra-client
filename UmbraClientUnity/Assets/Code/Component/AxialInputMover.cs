using UnityEngine;
using System.Collections;

public class AxialInputMover : MonoBehaviour {
    public delegate void MoveDelegate(Vector3 position, Vector3 velocity);
    public event MoveDelegate OnMove = delegate { };

    private float _speed = 100.0f;

	protected void Awake() {
        Enable();
	}

    protected void Destroy() {
        Disable();
    }

    public void Enable() {
        GameManager.Instance.Input.OnAxialInput += Move;
    }

    public void Disable(bool freeze = true) {
        GameManager.Instance.Input.OnAxialInput -= Move;
        if(freeze) rigidbody.velocity = Vector3.zero;
    }

    private void Move(float h, float v) {
        if(h != 0)
            h = (h < 0 ? -1 : 1);

        if(v != 0)
            v = (v < 0 ? -1 : 1);

        rigidbody.velocity = new Vector3(h * _speed, 0, v * _speed);

        OnMove(rigidbody.position, rigidbody.velocity);
    }
}
