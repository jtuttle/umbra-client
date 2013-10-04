using UnityEngine;
using System.Collections;
using System;

public class MapEnterState : BaseGameState {
    public PlayerView PlayerView { get; private set; }

    private Dungeon _dungeon;
    private tk2dSpriteCollectionData _tileset;
    private int _tileSize;

    private MapView _mapView;

    public MapEnterState(tk2dSpriteCollectionData tileset, int tileSize) 
        : base(GameStates.MapEnter) {

        _tileset = tileset;
        _tileSize = tileSize;

        _dungeon = GameManager.Instance.CurrentDungeon;

        _mapView = GameObject.FindObjectOfType(typeof(MapView)) as MapView;

        if(_mapView == null) throw new Exception("Game scene must contain MapView prefab.");
    }

    public override void EnterState() {
        base.EnterState();

        AdjustCamera();
        ShowMap();
        PlacePlayer();

        ExitState();
    }

    public override void ExitState() {

        base.ExitState();
    }

    public override void Dispose() {
        base.Dispose();

        PlayerView = null;
        _mapView = null;
        _tileset = null;
        _dungeon = null;
    }

    private void AdjustCamera() {
        Camera camera = GameManager.Instance.GameCamera;

        XY camPos = _mapView.TileCoordToWorldCoord(new XY(0, 0));
        camera.transform.position = new Vector3(camPos.X - (_tileSize / 2), camPos.Y - (_tileSize / 2), camera.transform.position.z);
    }

    private void ShowMap() {
        _mapView.SetSpriteData(64, _tileset);
        _mapView.SetDungeon(_dungeon);
    }

    private void PlacePlayer() {
        tk2dCamera camera = GameManager.Instance.GameCamera.GetComponent<tk2dCamera>();

        // map will specify some kind of starting position, for now just use dead center
        XY startCoord = new XY(camera.nativeResolutionWidth / 2, camera.nativeResolutionHeight / 2);

        GameObject player = UnityUtils.LoadResource<GameObject>("Prefabs/PlayerView", true);
        player.transform.position = new Vector3(startCoord.X, startCoord.Y, 0);

        // store playerView to pass on to other states
        PlayerView = player.GetComponent<PlayerView>();
    }
}
