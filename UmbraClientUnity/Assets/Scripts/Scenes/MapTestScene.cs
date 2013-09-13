using UnityEngine;
using System.Collections;

public class MapTestScene : MonoBehaviour {
    // same cam
    public MapViewCamera MapViewCamera;

    public MapView MapView;

    private int TILE_SIZE = 32;

    protected void Awake() {
        Map map = CreateMap();
        
        tk2dSpriteCollectionData spriteData = UnityUtils.LoadResource<tk2dSpriteCollectionData>("SpriteCollectionData/TestTileSet");

        XY camPos = MapView.TileCoordToWorldCoord(new XY(0, 0));
        MapView.SpriteCamera.transform.position = new Vector3(camPos.X - (TILE_SIZE / 2), camPos.Y - (TILE_SIZE / 2), MapViewCamera.transform.position.z);

        MapView.SetMap(map, TILE_SIZE, spriteData);

        MapView.ShowMap();
    }

    protected void Update() {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        XY delta = null;

        if(hAxis > 0)
            delta = new XY(TILE_SIZE * MapView.HorizontalTileCount, 0);

        if(hAxis < 0)
            delta = new XY(-TILE_SIZE * MapView.HorizontalTileCount, 0);

        if(vAxis > 0)
            delta = new XY(0, TILE_SIZE * MapView.VerticalTileCount);

        if(vAxis < 0)
            delta = new XY(0, -TILE_SIZE * MapView.VerticalTileCount);

        if(delta != null) {
            XY camPos = new XY((int)MapViewCamera.transform.position.x, (int)MapViewCamera.transform.position.y) + delta;
            XY newCamTilePos = MapView.WorldCoordToTileCoord(camPos.X, camPos.Y);

            if(newCamTilePos.X >= 0 && newCamTilePos.X < MapView.Map.Width - 1 && newCamTilePos.Y >= 0 && newCamTilePos.Y < MapView.Map.Height - 1)
                MapViewCamera.Move(delta);
        }
    }

    private Map CreateMap() {
        int width = 96;
        int height = 48;

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
