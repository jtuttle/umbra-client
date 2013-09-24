using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
    public delegate void AxialInputDelegate(float h, float v);
    public event AxialInputDelegate OnAxialInput = delegate { };

    public delegate void ButtonInputDelegate();
    public event ButtonInputDelegate OnAttackPress = delegate { };

    private bool _attacking;

	void Update() {
        // axial
        OnAxialInput(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // attack
        bool attackPressed = Input.GetButton("Attack");
        if(!attackPressed) _attacking = false;

        if(attackPressed && !_attacking) {
            _attacking = true;
            OnAttackPress();
        }
	}
}
