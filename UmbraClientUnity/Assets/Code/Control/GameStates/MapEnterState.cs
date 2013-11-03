using UnityEngine;
using System.Collections;
using System;

public class MapEnterState : BaseGameState {
    public PlayerView PlayerView { get; private set; }

    private MapView _mapView;

    private Dungeon _dungeon;

    public MapEnterState(Dungeon dungeon) 
        : base(GameStates.MapEnter) {

        _dungeon = dungeon;

        _mapView = GameObject.FindObjectOfType(typeof(MapView)) as MapView;

        if(_mapView == null) throw new Exception("Game scene must contain MapView prefab.");
    }

    public override void EnterState() {
        base.EnterState();

        SetCamera();
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
        _dungeon = null;
    }

    private void SetCamera() {
        int blockSize = GameConfig.BLOCK_SIZE;

        Camera cam = Camera.main;

        float camX = -(blockSize / 2) + (GameConfig.ROOM_WIDTH * blockSize) / 2;
        float camY = 180.0f;
        float camZ = 10.0f;

        float lookX = camX;
        float lookZ = -(blockSize / 2) + (GameConfig.ROOM_HEIGHT * blockSize) / 2;

        cam.transform.position = new Vector3(camX, camY, camZ);
        cam.transform.LookAt(new Vector3(lookX, 0, lookZ));
    }

    private void ShowMap() {
        _mapView.SetDungeon(_dungeon);
    }

    private void PlacePlayer() {
        int blockSize = GameConfig.BLOCK_SIZE;

        PlayerView = UnityUtils.LoadResource<GameObject>("Prefabs/Player", true).GetComponent<PlayerView>();

        float playerX = -(blockSize / 2) + (GameConfig.ROOM_WIDTH * blockSize) / 2;
        float playerZ = -(blockSize / 2) + (GameConfig.ROOM_HEIGHT * blockSize) / 2;
        float playerY = blockSize;

        PlayerView.transform.position = new Vector3(playerX, playerY, playerZ);
    }
}
