using UnityEngine;
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
        GameObject map = UnityUtils.LoadResource<GameObject>("Prefabs/Map", true);
        map.name = "Map";

        map.GetComponent<MapEntity>().SetMap(_map);

        GameManager.Instance.Map = map;
    }

    private void PlacePlayer() {
        GameObject player = UnityUtils.LoadResource<GameObject>("Prefabs/Player", true);
        player.name = "Player";

        MapEntity mapEntity = GameManager.Instance.Map.GetComponent<MapEntity>();
        Rect roomBounds = mapEntity.GetBoundsForCoord(GameManager.Instance.CurrentCoord);

        Vector2 mapCenter = roomBounds.center;
        player.transform.position = new Vector3(mapCenter.x, GameConfig.BLOCK_SIZE, mapCenter.y);

        GameManager.Instance.Player = player;
    }
}
