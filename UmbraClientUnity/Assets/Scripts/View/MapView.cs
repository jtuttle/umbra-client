using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MapView : MonoBehaviour {
    public tk2dCamera SpriteCamera;
    public MapViewCamera MapViewCamera;

    public MapTileViewBuffer BufferA;
    public MapTileViewBuffer BufferB;

    public int HorizontalTileCount { get { return SpriteCamera.nativeResolutionWidth / _tileSize; } }
    public int VerticalTileCount { get { return SpriteCamera.nativeResolutionHeight / _tileSize; } }

    public Map Map { get; private set; }
    private int _tileSize;
    private tk2dSpriteCollectionData _spriteData;

    private Dictionary<MapTile, MapTileView> _tileViews;
    private Queue<MapTileView> _tileViewPool;

    private MapTileViewBuffer _activeBuffer;
    private MapTileViewBuffer _inactiveBuffer;

    public void SetMap(Map map, int tileSize, tk2dSpriteCollectionData spriteData) {
        Map = map;
        _tileSize = tileSize;
        _spriteData = spriteData;

        BufferA.Setup(HorizontalTileCount, VerticalTileCount, tileSize, spriteData);
        BufferB.Setup(HorizontalTileCount, VerticalTileCount, tileSize, spriteData);

        _activeBuffer = BufferA;
        _inactiveBuffer = BufferB;

        MapViewCamera.OnMoveBegin += OnCameraMoveBegin;
        MapViewCamera.OnMoveEnd += OnCameraMoveEnd;
    }

    public void ShowMap() {
        List<MapTile> visibleMapTiles = GetVisibleMap();
        _activeBuffer.Show(visibleMapTiles);
    }

    private void OnCameraMoveBegin(XY delta) {
        Vector3 newPos = GetPositionForNewBuffer(delta);

        _inactiveBuffer.transform.position = newPos;

        XY bottomLeft = WorldCoordToTileCoord(newPos.x, newPos.y);
        XY topRight = bottomLeft + new XY(HorizontalTileCount - 1, VerticalTileCount - 1);

        _inactiveBuffer.Show(Map.GetMapArea(bottomLeft, topRight));
    }

    private void OnCameraMoveEnd(XY delta) {
        MapTileViewBuffer tempBuffer = _activeBuffer;

        _activeBuffer = _inactiveBuffer;
        _inactiveBuffer = tempBuffer;

        _inactiveBuffer.Hide();
    }

    private Vector3 GetPositionForNewBuffer(XY delta) {
        Vector3 activePos = _activeBuffer.gameObject.transform.position;

        if(delta.X > 0)
            return new Vector3(activePos.x + (HorizontalTileCount * _tileSize), activePos.y, activePos.z);
        if(delta.X < 0)
            return new Vector3(activePos.x - (HorizontalTileCount * _tileSize), activePos.y, activePos.z);
        if(delta.Y > 0)
            return new Vector3(activePos.x, activePos.y + (VerticalTileCount * _tileSize), activePos.z);
        if(delta.Y < 0)
            return new Vector3(activePos.x, activePos.y - (VerticalTileCount * _tileSize), activePos.z);

        throw new Exception("Camera didn't move");
    }

    private List<MapTile> GetVisibleMap() {
        Vector3 camPos = SpriteCamera.transform.position;

        XY bottomLeft = WorldCoordToTileCoord(camPos.x, camPos.y);
        XY topRight = bottomLeft + new XY(HorizontalTileCount - 1, VerticalTileCount - 1);

        return Map.GetMapArea(bottomLeft, topRight);
    }

    public XY TileCoordToWorldCoord(XY tileCoord) {
        return new XY(tileCoord.X * _tileSize, tileCoord.Y * _tileSize);
    }

    public XY WorldCoordToTileCoord(float worldX, float worldY) {
        int x = (int)(worldX / _tileSize) * _tileSize;
        int y = (int)(worldY / _tileSize) * _tileSize;
        return new XY(x / _tileSize, y / _tileSize);
    }
}
