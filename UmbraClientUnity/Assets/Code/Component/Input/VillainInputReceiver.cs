using UnityEngine;
using System.Collections;

public class VillainInputReceiver : MonoBehaviour {
	[RPC]
    public void SetVelocity(float horizontalAmount, float verticalAmount) {
        rigidbody.velocity = new Vector3(horizontalAmount, 0, verticalAmount);
    }
}
