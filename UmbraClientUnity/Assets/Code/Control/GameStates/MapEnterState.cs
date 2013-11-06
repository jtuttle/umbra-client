﻿using UnityEngine;
using System.Collections;
using System;

public class MapEnterState : BaseGameState {
    private Map _map;

    public MapEnterState(Map map) 
        : base(GameStates.MapEnter) {

        _map = map;
    }

    public override void EnterState() {
        base.EnterState();

        SetCamera();
        CreateMap();
        PlacePlayer();

        ExitState();
    }

    public override void Dispose() {
        _map = null;

        base.Dispose();
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

    private void CreateMap() {
        MapView mapView = UnityUtils.LoadResource<GameObject>("Prefabs/MapView", true).GetComponent<MapView>();
        mapView.gameObject.name = "MapView";

        mapView.SetMap(_map);
        mapView.UpdateRoomBounds(_map.Entrance.Coord);

        GameManager.Instance.MapView = mapView;
    }

    private void PlacePlayer() {
        PlayerView playerView = UnityUtils.LoadResource<GameObject>("Prefabs/PlayerView", true).GetComponent<PlayerView>();
        playerView.gameObject.name = "PlayerView";

        MapView mapView = GameManager.Instance.MapView;
        Vector2 mapCenter = mapView.RoomBounds.center;
        playerView.transform.position = new Vector3(mapCenter.x, GameConfig.BLOCK_SIZE, mapCenter.y);

        GameManager.Instance.PlayerView = playerView;
    }
}
