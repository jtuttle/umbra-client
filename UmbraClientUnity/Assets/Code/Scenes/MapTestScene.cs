using UnityEngine;
using System.Collections;

public class MapTestScene : MonoBehaviour {
    public GameObject PlayerView;
    public MapView MapView;

    public MapViewCamera MapViewCamera;

    private int TILE_SIZE = 64;

    void Awake() {
        Map map = CreateMap();
        
        tk2dSpriteCollectionData spriteData = UnityUtils.LoadResource<tk2dSpriteCollectionData>("SpriteCollectionData/TestTileSet");

        XY camPos = MapView.TileCoordToWorldCoord(new XY(0, 0));
        MapView.SpriteCamera.transform.position = new Vector3(camPos.X - (TILE_SIZE / 2), camPos.Y - (TILE_SIZE / 2), MapViewCamera.transform.position.z);

        MapView.SetSpriteData(TILE_SIZE, spriteData);
        MapView.SetDungeon(new DungeonGenerator().Generate(10));
    }

    private Map CreateMap() {
        int width = 48;
        int height = 24;

        Map map = new Map(width, height);

        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
                int sprite = ((x == 0 || x == width - 1 || y == 0 || y == height - 1) ? 1 : 0);
                map.AddMapTile(x, y, sprite);
            }
        }

        return map;
    }
}
