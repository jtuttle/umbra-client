using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapTileViewBuffer : MonoBehaviour {
    private List<MapTileView> _mapTileViews;

    public void Setup(int width, int height, int tileSize, tk2dSpriteCollectionData spriteData) {
        _mapTileViews = new List<MapTileView>();

        for(int y = 0; y < height; y++) {
            for(int x = 0; x < width; x++) {
                GameObject go = new GameObject();
                go.name = "MapTileView";
                go.transform.parent = gameObject.transform;
                go.transform.position = new Vector3(x * tileSize, y * tileSize, 0);

                MapTileView mapTileView = go.AddComponent<MapTileView>();
                mapTileView.Sprite = go.AddComponent<tk2dSprite>();
                mapTileView.Sprite.SetSprite(spriteData, 0);

                _mapTileViews.Add(mapTileView);
            }
        }
    }

    public void Show(List<MapTile> mapTiles) {
        for(int i = 0; i < mapTiles.Count; i++) {
            MapTileView mapTileView = _mapTileViews[i];
            mapTileView.Sprite.SetSprite(mapTiles[i].SpriteIndex);
        }

        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
