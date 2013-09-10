using UnityEngine;
using System.Collections;

public class MapTestScene : MonoBehaviour {
    public MapView MapView;

    protected void Awake() {
        Map map = CreateMap();

        tk2dSpriteCollectionData spriteData = UnityUtils.LoadResource<tk2dSpriteCollectionData>("SpriteCollectionData/TestTileSet");

        MapView.SetMap(map, spriteData);
        MapView.ShowMap();
    }


    private Map CreateMap() {
        Map map = new Map(100, 100, 32, 32);

        for(int x = 0; x < 100; x++) {
            for(int y = 0; y < 100; y++) {
                map.AddMapTile(x, y, 0);
            }
        }

        return map;
    }
}
