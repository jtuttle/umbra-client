using UnityEngine;
using System.Collections;
using System;

public class MapEnterState : BaseGameState {
    public Vector3 PlayerPosition { get; private set; }

    private Map _map;

    private MapView _mapView;

    public MapEnterState(Map map) 
        : base(GameStates.MapEnter) {

        _map = map;

        _mapView = GameObject.FindObjectOfType(typeof(MapView)) as MapView;
        
        if(_mapView == null) throw new Exception("Game scene must contain MapView prefab.");
    }

    public override void EnterState() {
        base.EnterState();

        _mapView.UpdateRoomBounds(_map.Entrance.Coord);

        SetCamera();
        ShowMap();

        Vector2 mapCenter = _mapView.RoomBounds.center;
        PlayerPosition = new Vector3(mapCenter.x, GameConfig.BLOCK_SIZE, mapCenter.y);

        ExitState();
    }

    public override void ExitState() {

        base.ExitState();
    }

    public override void Dispose() {
        base.Dispose();

        _mapView = null;
        _map = null;
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
        _mapView.SetMap(_map);
    }
}
