using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using DungeonNode = GridNode<DungeonRoom, DungeonPath>;

public class MapView : MonoBehaviour {
    public tk2dCamera SpriteCamera;
    public MapViewCamera MapViewCamera;

    public MapTileViewBuffer BufferA;
    public MapTileViewBuffer BufferB;

    public Dungeon Dungeon { get; private set; }
    public int TileSize { get; private set; }

    public int HorizontalTileCount { get; private set; }
    public int VerticalTileCount { get; private set; }

    private MapTileViewBuffer _activeBuffer;
    private MapTileViewBuffer _inactiveBuffer;

    private XY _currentNodePosition;

    public void SetSpriteData(int tileSize, tk2dSpriteCollectionData spriteData) {
        TileSize = tileSize;

        HorizontalTileCount = SpriteCamera.nativeResolutionWidth / TileSize;
        VerticalTileCount = SpriteCamera.nativeResolutionHeight / TileSize;

        BufferA.Setup(HorizontalTileCount, VerticalTileCount, tileSize, spriteData);
        BufferB.Setup(HorizontalTileCount, VerticalTileCount, tileSize, spriteData);

        _activeBuffer = BufferA;
        _inactiveBuffer = BufferB;
        _inactiveBuffer.gameObject.SetActive(false);

        MapViewCamera.OnMoveBegin += OnCameraMoveBegin;
        MapViewCamera.OnMoveEnd += OnCameraMoveEnd;
    }

    public void SetDungeon(Dungeon dungeon) {
        Dungeon = dungeon;

        ShowDungeonRoom(Dungeon.Entrance, _activeBuffer);

        _currentNodePosition = Dungeon.Entrance.Coord;
    }

    public void ShowDungeonRoom(DungeonNode dungeonNode, MapTileViewBuffer buffer) {
        List<MapTile> roomTiles = TilesForDungeonNode(dungeonNode);
        buffer.Show(roomTiles);
    }

    private void OnCameraMoveBegin(XY delta) {
        Vector3 newPos = GetPositionForNewBuffer(DirectionFromDelta(delta));

        _inactiveBuffer.transform.position = newPos;

        GridDirection moveDirection = DirectionFromDelta(delta);
        DungeonNode nextNode = Dungeon.Graph.GetNodeByCoord(NextCoord(_currentNodePosition, moveDirection));

        if(nextNode != null)
            ShowDungeonRoom(nextNode, _inactiveBuffer);
    }

    private void OnCameraMoveEnd(XY delta) {
        MapTileViewBuffer tempBuffer = _activeBuffer;

        _activeBuffer = _inactiveBuffer;
        _inactiveBuffer = tempBuffer;

        _inactiveBuffer.Hide();

        _currentNodePosition = NextCoord(_currentNodePosition, DirectionFromDelta(delta));
    }

    private Vector3 GetPositionForNewBuffer(GridDirection direction) {
        Vector3 activePos = _activeBuffer.gameObject.transform.position;

        if(direction == GridDirection.N)
            return new Vector3(activePos.x, activePos.y + SpriteCamera.nativeResolutionHeight, activePos.z);
        if(direction == GridDirection.E)
            return new Vector3(activePos.x + SpriteCamera.nativeResolutionWidth, activePos.y, activePos.z);
        if(direction == GridDirection.S)
            return new Vector3(activePos.x, activePos.y - SpriteCamera.nativeResolutionHeight, activePos.z);
        if(direction == GridDirection.W)
            return new Vector3(activePos.x - SpriteCamera.nativeResolutionWidth, activePos.y, activePos.z);

        throw new Exception("Camera didn't move");
    }

    public XY TileCoordToWorldCoord(XY tileCoord) {
        return new XY(tileCoord.X * TileSize, tileCoord.Y * TileSize);
    }

    public XY WorldCoordToTileCoord(float worldX, float worldY) {
        int x = (int)(worldX / TileSize) * TileSize;
        int y = (int)(worldY / TileSize) * TileSize;
        return new XY(x / TileSize, y / TileSize);
    }

    private GridDirection DirectionFromDelta(XY delta) {
        if(delta.Y > 0) return GridDirection.N;
        if(delta.X > 0) return GridDirection.E;
        if(delta.Y < 0) return GridDirection.S;
        if(delta.X < 0) return GridDirection.W;
        throw new Exception("Unable to get direction from delta");
    }

    private XY NextCoord(XY coord, GridDirection direction) {
        if(direction == GridDirection.N) return coord + new XY(0, 1);
        if(direction == GridDirection.E) return coord + new XY(1, 0);
        if(direction == GridDirection.S) return coord - new XY(0, 1);
        if(direction == GridDirection.W) return coord - new XY(1, 0);
        throw new Exception("Unable to get next coord");
    }

    private List<MapTile> TilesForDungeonNode(DungeonNode node) {
        List<MapTile> tiles = new List<MapTile>();
        
        for(int y = 0; y < 12; y++) {
            for(int x = 0; x < 16; x++) {
                int spriteIndex = 0;

                if(y == 0) {
                    if(x < 6 || x > 9 || !node.Edges.ContainsKey(GridDirection.S))
                        spriteIndex = 1;
                } else if(y == 11) {
                    if(x < 6 || x > 9 || !node.Edges.ContainsKey(GridDirection.N))
                        spriteIndex = 1;
                }

                if(x == 0) {
                    if(y < 4 || y > 7 || !node.Edges.ContainsKey(GridDirection.W))
                        spriteIndex = 1;
                } else if(x == 15) {
                    if(y < 4 || y > 7 || !node.Edges.ContainsKey(GridDirection.E))
                        spriteIndex = 1;
                }

                
                tiles.Add(new MapTile(x, y, spriteIndex));
            }
        }

        return tiles;
    }
}
