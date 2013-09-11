using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class TileCamera : MonoBehaviour {
    public bool Moving;

    protected void Awake() {
        Moving = false;
    }

    public void Move(Vector3 destination) {
        if(Moving) return;

        TweenParms parms = new TweenParms();
        parms.Ease(EaseType.Linear);
        parms.Prop("position", destination);
        parms.OnComplete(OnMoveComplete);

        Moving = true;

        HOTween.To(gameObject.transform, 0.5f, parms);
    }

    private void OnMoveComplete(TweenEvent e) {
        Moving = false;
    }
}
