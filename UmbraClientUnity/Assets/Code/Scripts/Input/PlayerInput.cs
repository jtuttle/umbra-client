using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {
    public delegate void PlayerMoveDelegate(Vector3 position, Vector3 velocity);
    public event PlayerMoveDelegate OnPlayerMove = delegate { };

    public delegate void PlayerAttackDelegate();
    public event PlayerAttackDelegate OnPlayerAttack = delegate { };

    private float _speed = 300.0f;

    private bool _attacking;

    void Update() {
        bool attackPressed = Input.GetButton("Attack");
        if(!attackPressed) _attacking = false;

        if(attackPressed && !_attacking) {
            _attacking = true;
            OnPlayerAttack();
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if(h != 0)
            h = (h < 0 ? -1 : 1);

        if(v != 0)
            v = (v < 0 ? -1 : 1);

        rigidbody.velocity = new Vector3(h * _speed, v * _speed, 0);

        if(rigidbody.velocity != Vector3.zero)
            OnPlayerMove(gameObject.transform.position, rigidbody.velocity);
    }

    public void Disable() {
        enabled = false;
        rigidbody.velocity = Vector3.zero;
    }

    public void Enable() {
        enabled = true;
    }
}
