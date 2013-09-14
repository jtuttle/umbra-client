using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {
    private float _speed = 4.0f;

    protected void Update() {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if(h != 0)
            h = (h < 0 ? -1 : 1);

        if(v != 0)
            v = (v < 0 ? -1 : 1);

        gameObject.transform.position += new Vector3(h * _speed, v * _speed, 0);
    }
}
