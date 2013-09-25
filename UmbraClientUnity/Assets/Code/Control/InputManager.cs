using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
    public delegate void AxialInputDelegate(float h, float v);
    public event AxialInputDelegate OnAxialInput = delegate { };

    public delegate void ButtonInputDelegate();
    public event ButtonInputDelegate OnAttackPress = delegate { };
    public event ButtonInputDelegate OnSpecialPress = delegate { };

    private bool _attack;
    private bool _special;

	void Update() {
        // axial
        OnAxialInput(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // attack
        bool attackPressed = Input.GetButton("Attack");
        if(!attackPressed) _attack = false;

        if(attackPressed && !_attack) {
            _attack = true;
            OnAttackPress();
        }

        // special
        bool specialPressed = Input.GetButton("Special");
        if(!specialPressed) _special = false;

        if(specialPressed && !_special) {
            _special = true;
            OnSpecialPress();
        }
	}
}
