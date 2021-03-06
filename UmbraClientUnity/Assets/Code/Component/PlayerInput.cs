﻿using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {
    private RigidBodyMover _mover;
    private MeleeAttacker _meleeAttacker;

    protected void Awake() {
        _mover = gameObject.GetComponent<RigidBodyMover>();
        _meleeAttacker = gameObject.GetComponent<MeleeAttacker>();

        Enable();
	}

    protected void Destroy() {
        Disable();
    }

    public void Enable() {
        GameManager.Instance.Input.OnAxialInput += Move;
        GameManager.Instance.Input.GetButton(ButtonId.Attack).OnPress += MeleeAttack;
    }

    public void Disable(bool freeze = true) {
        GameManager.Instance.Input.OnAxialInput -= Move;
        GameManager.Instance.Input.GetButton(ButtonId.Attack).OnPress -= MeleeAttack;

        if(freeze) rigidbody.velocity = Vector3.zero;
    }

    private void Move(float h, float v) {
        /*
        if(h != 0)
            h = (h < 0 ? -1 : 1);

        if(v != 0)
            v = (v < 0 ? -1 : 1);
        */

        _mover.Move(h, v);
    }

    private void MeleeAttack() {
        _meleeAttacker.Attack();
    }
}
