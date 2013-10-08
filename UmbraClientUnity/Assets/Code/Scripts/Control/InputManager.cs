using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
    public delegate void AxialInputDelegate(float h, float v);
    public event AxialInputDelegate OnAxialInput = delegate { };

    public delegate void ButtonInputDelegate();
    public event ButtonInputDelegate OnAttackPress = delegate { };
    public event ButtonInputDelegate OnSpecialPress = delegate { };
    public event ButtonInputDelegate OnMapViewPress = delegate { };

    private bool _attack;
    private bool _special;
    private bool _mapView;

    private float _dead = 0.2f;

	void Update() {
        // manual deadzones because built-in ones aren't working
        float h = Input.GetAxis("Horizontal");
        h = (Mathf.Abs(h) < _dead ? 0 : h);

        float v = Input.GetAxis("Vertical");
        v = (Mathf.Abs(v) < _dead ? 0 : v);

        // axial
        OnAxialInput(h, v);

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

        // map view
        bool mapViewPressed = Input.GetButton("MapView");
        if(!mapViewPressed) _mapView = false;

        if(mapViewPressed && !_mapView) {
            _mapView = true;
            OnMapViewPress();
        }
	}
}
