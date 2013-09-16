using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class MapViewCamera : MonoBehaviour {
    public delegate void MapViewCameraMoveDelegate(XY delta);

    public MapViewCameraMoveDelegate OnMoveBegin = delegate { };
    public MapViewCameraMoveDelegate OnMoveEnd = delegate { };

    public GameObject Player;
    public MapView MapView;

    public bool Moving;

    private tk2dCamera _tk2dCameraRef;

    protected void Awake() {
        Moving = false;

        _tk2dCameraRef = gameObject.GetComponent<tk2dCamera>();

        Player.GetComponent<PlayerInput>().OnPlayerMove += OnPlayerMove;
    }

    protected void Destroy() {
        Player.GetComponent<PlayerInput>().OnPlayerMove -= OnPlayerMove;
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

    private void OnPlayerMove(Vector3 newPos) {
        XY delta = null;

        if(newPos.x > _tk2dCameraRef.transform.position.x + _tk2dCameraRef.nativeResolutionWidth)
            delta = new XY(MapView.TileSize * MapView.HorizontalTileCount, 0);
        else if(newPos.x < _tk2dCameraRef.transform.position.x)
            delta = new XY(-MapView.TileSize * MapView.HorizontalTileCount, 0);
        else if(newPos.y > _tk2dCameraRef.transform.position.y + _tk2dCameraRef.nativeResolutionHeight)
            delta = new XY(0, MapView.TileSize * MapView.VerticalTileCount);
        else if(newPos.y < _tk2dCameraRef.transform.position.y)
            delta = new XY(0, -MapView.TileSize * MapView.VerticalTileCount);

        if(delta != null)
            Move(delta);
    }
}
