using UnityEngine;
using System.Collections;

public class PlayerView : MonoBehaviour {
    public delegate void PlayerMoveDelegate(Vector3 position, Vector3 velocity);
    public event PlayerMoveDelegate OnPlayerMove = delegate { };

    public delegate void PlayerAttackDelegate();
    public event PlayerAttackDelegate OnPlayerAttack = delegate { };

    private MeleeAttacker _meleeAttacker;

    private float _speed = 100.0f;

    void Awake() {
        _meleeAttacker = GetComponent<MeleeAttacker>() as MeleeAttacker;
    }

    void Destroy() {
        
    }

    public void Move(float h, float v) {
        if(h != 0)
            h = (h < 0 ? -1 : 1);

        if(v != 0)
            v = (v < 0 ? -1 : 1);

        //rigidbody.velocity = new Vector3(h * _speed, v * _speed, 0);
        rigidbody.velocity = new Vector3(h * _speed, 0, v * _speed);

        if(rigidbody.velocity != Vector3.zero)
            OnPlayerMove(gameObject.transform.position, rigidbody.velocity);

        if(_meleeAttacker != null)
            _meleeAttacker.UpdateAttackColliderPosition(transform.position, rigidbody.velocity);
    }

    public void Attack() {
        _meleeAttacker.Attack();

        OnPlayerAttack();
    }

    public void Freeze() {
        rigidbody.velocity = Vector3.zero;

        // probably freeze animation too
    }

    public void Unfreeze() {
        // kick animation to a neutral frame
    }
}
