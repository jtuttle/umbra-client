using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MapView : MonoBehaviour {
    public tk2dCamera SpriteCamera;

    public int HorizontalTileCount { get { return SpriteCamera.nativeResolutionWidth / _tileSize; } }
    public int VerticalTileCount { get { return SpriteCamera.nativeResolutionHeight / _tileSize; } }

    private Map _map;
    private int _tileSize;
    private tk2dSpriteCollectionData _spriteData;

    private MapTileView[,] _tileViews;

    protected void Update() {
        UpdateActive();
    }

    public void SetMap(Map map, int tileSize, tk2dSpriteCollectionData spriteData) {
        _map = map;
        _tileSize = tileSize;
        _spriteData = spriteData;

        _tileViews = new MapTileView[map.Width, map.Height];
    }

    public void ShowMap() {
        for(int x = 0; x < _map.Width; x++) {
            for(int y = 0; y < _map.Width; y++) {
                GameObject go = new GameObject();
                go.name = "MapTileView";
                go.transform.parent = gameObject.transform;
                go.transform.position = new Vector3(x * _tileSize, y * _tileSize, 0);
                go.SetActive(false);

                MapTileView mapTileView = go.AddComponent<MapTileView>();
                mapTileView.Sprite = go.AddComponent<tk2dSprite>();
                mapTileView.MapTile = _map.GetMapTile(x, y);

                mapTileView.Sprite.SetSprite(_spriteData, mapTileView.MapTile.SpriteIndex);

                _tileViews[x, y] = mapTileView;
            }
        }
    }

    public void UpdateActive() {
        Vector3 camPos = SpriteCamera.transform.position;

        XY bottomLeft = WorldCoordToTileCoord(camPos.x, camPos.y);
        XY topRight = WorldCoordToTileCoord(camPos.x + SpriteCamera.nativeResolutionWidth, camPos.y + SpriteCamera.nativeResolutionHeight);

        int t = 4;

        for(int x = Math.Max(0, bottomLeft.X - t); x <= Math.Min(topRight.X + t, _map.Width - 1); x++) {
            for(int y = Math.Max(0, bottomLeft.Y - t); y <= Math.Min(topRight.Y + t, _map.Height - 1); y++) {
                if(x < bottomLeft.X - 2 || x > topRight.X + 2 || y < bottomLeft.Y - 2 || y > topRight.Y + 2)
                    _tileViews[x, y].gameObject.SetActive(false);
                else
                    _tileViews[x, y].gameObject.SetActive(true);
            }
        }
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
