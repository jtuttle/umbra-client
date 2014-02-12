using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using MapNode = GridNode<MapRoom, MapPath>;

public class MapEntity : MonoBehaviour {
    public Dictionary<XY, MapRoomEntity> MapRooms;

    public Map MapModel { get; private set; }

    protected void Awake() {
        MapRooms = new Dictionary<XY, MapRoomEntity>();
    }

    public void SetMap(Map mapModel) {
        MapModel = mapModel;

        CreateMap();
    }

    public Rect GetBoundsForCoord(XY coord, int blockMargin = 0) {
        // caching doesn't work anymore with the addition of margin
        // return cached value if possible
        //if(MapRooms.ContainsKey(coord))
        //    return MapRooms[coord].RoomBounds;

        int blockSize = GameConfig.BLOCK_SIZE;
        float margin = blockMargin * blockSize;
        float roomWidth = (GameConfig.ROOM_WIDTH * blockSize) - (margin * 2);
        float roomHeight = (GameConfig.ROOM_HEIGHT * blockSize) - (margin * 2);

        float left = (coord.X * roomWidth) - (blockSize / 2.0f) + margin;
        float top = (coord.Y * roomHeight) - (blockSize / 2.0f) + margin;

        return new Rect(left, top, roomWidth, roomHeight);
    }

    public XY GetCoordFromPosition(Vector3 position) {
        int coordX = (int)Mathf.Floor(position.x / (GameConfig.ROOM_WIDTH * GameConfig.BLOCK_SIZE));
        int coordZ = (int)Mathf.Floor(position.z / (GameConfig.ROOM_HEIGHT * GameConfig.BLOCK_SIZE));
        return new XY(coordX, coordZ);
    }

    private void CreateMap() {
        foreach(MapNode node in MapModel.Graph.BreadthFirstSearch(MapModel.Entrance))
            CreateRoom(node);
    }

    private void CreateRoom(MapNode node) {
        MapRoomEntity mapRoomEntity = UnityUtils.LoadResource<GameObject>("Prefabs/MapRoom", true).GetComponent<MapRoomEntity>();
        mapRoomEntity.Initialize(node.Coord, GetBoundsForCoord(node.Coord));
        mapRoomEntity.transform.parent = gameObject.transform;

        MapRooms[mapRoomEntity.Coord] = mapRoomEntity;

        int roomWidth = GameConfig.ROOM_WIDTH;
        int roomHeight = GameConfig.ROOM_HEIGHT;
        int blockSize = GameConfig.BLOCK_SIZE;

        XY start = new XY(node.Coord.X * roomWidth * blockSize, node.Coord.Y * roomHeight * blockSize);

        for(int y = 0; y < roomHeight; y++) {
            for(int x = 0; x < roomWidth; x++) {
                int blockX = start.X + x * blockSize;
                int blockZ = start.Y + y * blockSize;

                GameObject floorBlock = UnityUtils.LoadResource<GameObject>("Prefabs/MapBlock", true);
                floorBlock.transform.position = new Vector3(blockX, 0, blockZ);
                floorBlock.transform.localScale = new Vector3(blockSize, blockSize / 2.0f, blockSize);
                floorBlock.transform.parent = mapRoomEntity.transform;
                floorBlock.name = "FloorBlock";

                if(y == 0) {
                    if(x < 6 || x > 9 || !node.Edges.ContainsKey(GridDirection.S))
                        CreateWall(blockX, blockZ, mapRoomEntity.transform);
                } else if(y == roomHeight - 1) {
                    if(x < 6 || x > 9 || !node.Edges.ContainsKey(GridDirection.N))
                        CreateWall(blockX, blockZ, mapRoomEntity.transform);
                } else if(x == 0) {
                    if(y < 4 || y > 7 || !node.Edges.ContainsKey(GridDirection.W))
                        CreateWall(blockX, blockZ, mapRoomEntity.transform);
                } else if(x == roomWidth - 1) {
                    if(y < 4 || y > 7 || !node.Edges.ContainsKey(GridDirection.E))
                        CreateWall(blockX, blockZ, mapRoomEntity.transform);
                }
            }
        }

        Rect roomBounds = GetBoundsForCoord(node.Coord);

        GameObject floor = UnityUtils.LoadResource<GameObject>("Prefabs/MapFloor", true);
        floor.transform.position = new Vector3(roomBounds.center.x, 0, roomBounds.center.y);
        floor.transform.localScale = new Vector3(roomBounds.width, 0.5f, roomBounds.height);
        floor.transform.parent = mapRoomEntity.transform;
        floor.name = "MapFloor";
    }

    private void CreateWall(int x, int z, Transform parent) {
        int blockSize = GameConfig.BLOCK_SIZE;

        for(int i = 0; i < 4; i++) {
            GameObject wallBlock = UnityUtils.LoadResource<GameObject>("Prefabs/MapBlock", true);
            wallBlock.transform.position = new Vector3(x, i * blockSize, z);
            wallBlock.transform.localScale = new Vector3(blockSize, blockSize, blockSize);
            wallBlock.transform.parent = parent;
            wallBlock.name = "WallBlock";
        }
    }
}
