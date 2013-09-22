using UnityEngine;
using System.Collections;

public class MapTileView : MonoBehaviour {
    public tk2dSprite Sprite;

    public MapTile MapTile { get; private set; }

    public void UpdateMapTile(MapTile mapTile) {
        MapTile = mapTile;
        Sprite.SetSprite(mapTile.SpriteIndex);
    }
}
