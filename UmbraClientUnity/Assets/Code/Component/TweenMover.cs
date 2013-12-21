using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class TweenMover : MonoBehaviour {
    public delegate void MoveDelegate(Vector3 from, Vector3 to);

    public MoveDelegate OnMoveBegin = delegate { };
    public MoveDelegate OnMoveEnd = delegate { };

    public bool Moving { get; private set; }

    public void Awake() {
        Moving = false;
    }

    public void Move(XY delta) {
        Vector3 pos = gameObject.transform.position;
        Vector3 target = new Vector3(pos.x + delta.X, pos.y, pos.z + delta.Y);
        Move(target);
    }

    public void Move(Vector3 target) {
        if(Moving) return;

        Vector3 from = gameObject.transform.position;

        OnMoveBegin(from, target);

        TweenParms parms = new TweenParms();
        parms.Ease(EaseType.Linear);
        parms.Prop("position", target);
        parms.OnComplete(OnMoveComplete, from, target);

        Moving = true;

        HOTween.To(gameObject.transform, 0.5f, parms);
    }

    private void OnMoveComplete(TweenEvent e) {
        Vector3 from = (Vector3)e.parms[0];
        Vector3 to = (Vector3)e.parms[1];

        OnMoveEnd(from, to);

        Moving = false;
    }
}
