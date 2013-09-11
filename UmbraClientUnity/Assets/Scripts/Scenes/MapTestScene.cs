using UnityEngine;
using System.Collections;

public class MapTestScene : MonoBehaviour {
    // same cam
    public tk2dCamera SpriteCamera;
    public TileCamera TileCamera;

    public MapView MapView;

    protected void Awake() {
        Map map = CreateMap();

        tk2dSpriteCollectionData spriteData = UnityUtils.LoadResource<tk2dSpriteCollectionData>("SpriteCollectionData/TestTileSet");

        MapView.SetMap(map, 32, spriteData);

        XY camPos = MapView.TileCoordToWorldCoord(new XY(2, 3));
        MapView.SpriteCamera.transform.position = new Vector3(camPos.X, camPos.Y, SpriteCamera.transform.position.z);

        MapView.ShowMap();
    }

    protected void Update() {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        if(hAxis > 0)
            TileCamera.Move(TileCamera.transform.position + new Vector3(32, 0, 0));

        if(hAxis < 0)
            TileCamera.Move(TileCamera.transform.position - new Vector3(32, 0, 0));

        if(vAxis > 0)
            TileCamera.Move(TileCamera.transform.position + new Vector3(0, 32, 0));

        if(vAxis < 0)
            TileCamera.Move(TileCamera.transform.position - new Vector3(0, 32, 0));
    }

    private Map CreateMap() {
        Map map = new Map(100, 100);

        for(int x = 0; x < 100; x++) {
            for(int y = 0; y < 100; y++) {
                map.AddMapTile(x, y, 0);
            }
        }

        return map;
    }
}
