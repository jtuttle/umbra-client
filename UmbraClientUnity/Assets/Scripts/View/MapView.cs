using UnityEngine;
using System.Collections;

public class MapView : MonoBehaviour {
    public tk2dCamera SpriteCamera;
    
    private Map _map;
    private int _tileSize;
    private tk2dSpriteCollectionData _spriteData;

    public void SetMap(Map map, int tileSize, tk2dSpriteCollectionData spriteData) {
        _map = map;
        _tileSize = tileSize;
        _spriteData = spriteData;
    }

    public void ShowMap() {
        Vector3 camPos = SpriteCamera.transform.position;
        XY camTileCoord = WorldCoordToTileCoord(camPos.x, camPos.y);

        int xMax = camTileCoord.X + (SpriteCamera.nativeResolutionWidth / _tileSize) + 1;
        int yMax = camTileCoord.Y + (SpriteCamera.nativeResolutionHeight / _tileSize) + 1;

        for(int x = camTileCoord.X - 1; x < xMax; x++) {
            for(int y = camTileCoord.Y - 1; y < yMax; y++) {
                MapTile mapTile = _map.GetMapTile(x, y);

                if(mapTile != null) {
                    GameObject go = new GameObject();
                    go.name = mapTile.ToString();
                    go.transform.parent = gameObject.transform;
                    go.transform.position = new Vector3((_tileSize / 2) + x * _tileSize, (_tileSize / 2) + y * _tileSize, 0);

                    tk2dSprite sprite = go.AddComponent<tk2dSprite>();
                    sprite.SetSprite(_spriteData, mapTile.SpriteIndex);
                }
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
