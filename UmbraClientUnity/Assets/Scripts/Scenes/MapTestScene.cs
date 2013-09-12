using UnityEngine;
using System.Collections;

public class MapTestScene : MonoBehaviour {
    // same cam
    public tk2dCamera SpriteCamera;
    public TileCamera TileCamera;

    public MapView MapView;

    private int TILE_SIZE = 32;

    protected void Awake() {
        Map map = CreateMap();
        
        tk2dSpriteCollectionData spriteData = UnityUtils.LoadResource<tk2dSpriteCollectionData>("SpriteCollectionData/TestTileSet");

        MapView.SetMap(map, TILE_SIZE, spriteData);

        XY camPos = MapView.TileCoordToWorldCoord(new XY(0, 0));
        MapView.SpriteCamera.transform.position = new Vector3(camPos.X - (TILE_SIZE / 2), camPos.Y - (TILE_SIZE / 2), SpriteCamera.transform.position.z);

        MapView.ShowMap();
    }

    protected void Update() {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        Debug.Log(MapView.HorizontalTileCount);

        if(hAxis > 0)
            TileCamera.Move(new Vector3(TILE_SIZE * MapView.HorizontalTileCount, 0, 0));

        if(hAxis < 0)
            TileCamera.Move(new Vector3(-TILE_SIZE * MapView.HorizontalTileCount, 0, 0));

        if(vAxis > 0)
            TileCamera.Move(new Vector3(0, TILE_SIZE * MapView.VerticalTileCount, 0));

        if(vAxis < 0)
            TileCamera.Move(new Vector3(0, -TILE_SIZE * MapView.VerticalTileCount, 0));
    }

    private Map CreateMap() {
        Map map = new Map(100, 100);

        for(int x = 0; x < 100; x++) {
            for(int y = 0; y < 100; y++) {
                int sprite = ((x == 0 || x == 100 || y == 0 || y == 100) ? 1 : 0);
                map.AddMapTile(x, y, sprite);
            }
        }

        return map;
    }
}
