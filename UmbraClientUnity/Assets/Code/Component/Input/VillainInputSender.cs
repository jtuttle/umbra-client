using System.Collections;
using uLink;
using UnityEngine;

public class VillainInputSender : uLink.MonoBehaviour {
    public float Velocity = 10;

    protected void Update() {
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        if(hInput != 0 || vInput != 0) {
            //Debug.Log("Sending X: " + hInput * arrowKeysVelocity);
            networkView.RPC("SetVelocity", uLink.NetworkPlayer.server, hInput * Velocity, vInput * Velocity);
        }
    }
}
