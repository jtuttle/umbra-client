using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class MapViewCamera : MonoBehaviour {
    public delegate void MapViewCameraMoveDelegate(XY delta);

    public MapViewCameraMoveDelegate OnMoveBegin = delegate { };
    public MapViewCameraMoveDelegate OnMoveEnd = delegate { };

    public bool Moving;

    protected void Awake() {
        Moving = false;
    }

    public void Move(XY delta) {
        if(Moving) return;

        OnMoveBegin(delta);

        Vector3 goPos = gameObject.transform.position;

        TweenParms parms = new TweenParms();
        parms.Ease(EaseType.Linear);
        parms.Prop("position", goPos + new Vector3(delta.X, delta.Y, 0));
        parms.OnComplete(OnMoveComplete, delta);

        Moving = true;

        HOTween.To(gameObject.transform, 0.5f, parms);
    }

    private void OnMoveComplete(TweenEvent e) {
        Moving = false;

        XY delta = (XY)e.parms[0];

        OnMoveEnd(delta);
    }
}
