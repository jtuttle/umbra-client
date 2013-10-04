using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class MapViewCamera : MonoBehaviour {
    public delegate void MapViewCameraMoveDelegate(XY delta);

    public MapViewCameraMoveDelegate OnMoveBegin = delegate { };
    public MapViewCameraMoveDelegate OnMoveEnd = delegate { };

    public MapView MapView;

    public bool Moving { get; private set; }

    public XY HalfScreen { get { return new XY(_spriteCamera.nativeResolutionWidth / 2, _spriteCamera.nativeResolutionHeight / 2); } }
    public XY FullScreen { get { return new XY(_spriteCamera.nativeResolutionWidth, _spriteCamera.nativeResolutionHeight); } }

    private tk2dCamera _spriteCamera;

    void Awake() {
        Moving = false;

        _spriteCamera = gameObject.GetComponent<tk2dCamera>();
    }

    public void CoverPosition(Vector3 position) {
        XY delta = null;

        if(position.x > _spriteCamera.transform.position.x + _spriteCamera.nativeResolutionWidth)
            delta = new XY(MapView.TileSize * MapView.HorizontalTileCount, 0);
        else if(position.x < _spriteCamera.transform.position.x)
            delta = new XY(-MapView.TileSize * MapView.HorizontalTileCount, 0);
        else if(position.y > _spriteCamera.transform.position.y + _spriteCamera.nativeResolutionHeight)
            delta = new XY(0, MapView.TileSize * MapView.VerticalTileCount);
        else if(position.y < _spriteCamera.transform.position.y)
            delta = new XY(0, -MapView.TileSize * MapView.VerticalTileCount);

        if(delta != null)
            Move(delta);
    }

    public void Move(XY delta) {
        if(Moving) return;

        Vector3 goPos = gameObject.transform.position;
        Vector3 newPos = goPos + new Vector3(delta.X, delta.Y, 0);

        OnMoveBegin(delta);
        
        TweenParms parms = new TweenParms();
        parms.Ease(EaseType.Linear);
        parms.Prop("position", newPos);
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
