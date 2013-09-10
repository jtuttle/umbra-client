using UnityEngine;
using System.Collections;

public class MapView : MonoBehaviour {
    public tk2dCamera SpriteCamera;
    
    private Map _map;
    private tk2dSpriteCollectionData _spriteData;

    public void SetMap(Map map, tk2dSpriteCollectionData spriteData) {
        _map = map;
        _spriteData = spriteData;
    }

    public void ShowMap() {
        int hTiles = (SpriteCamera.nativeResolutionWidth / _map.TileWidth) + 1;
        int vTiles = (SpriteCamera.nativeResolutionHeight / _map.TileHeight) + 1;

        for(int x = -1; x < hTiles; x++) {
            for(int y = -1; y < vTiles; y++) {
                MapTile mapTile = _map.GetMapTile(x, y);

                if(mapTile != null) {
                    GameObject go = new GameObject();
                    go.name = mapTile.ToString();
                    go.transform.parent = gameObject.transform;
                    go.transform.position = new Vector3((_map.TileWidth / 2) + x * _map.TileWidth, (_map.TileHeight / 2) + y * _map.TileHeight, 0);

                    tk2dSprite sprite = go.AddComponent<tk2dSprite>();
                    sprite.SetSprite(_spriteData, mapTile.SpriteIndex);
                }
            }
        }
    }
}
