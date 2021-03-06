﻿using UnityEngine;
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

        DrawMap();
    }

    public Rect GetBoundsForCoord(XY coord, float margin = 0) {
        // return cached value if possible
        //if(MapRooms.ContainsKey(coord))
        //    return MapRooms[coord].RoomBounds;

        int blockSize = GameConfig.BLOCK_SIZE;
        
        float roomWidth = (GameConfig.ROOM_WIDTH * blockSize) - (margin * 2); // WTF??
        float roomHeight = (GameConfig.ROOM_HEIGHT * blockSize) - (margin * 2);

        float left = coord.X * roomWidth - (blockSize / 2) + margin;
        float top = coord.Y * roomHeight - (blockSize / 2) + margin;

        return new Rect(left, top, roomWidth, roomHeight);
    }

    public XY GetCoordFromPosition(Vector3 position) {
        int coordX = (int)Mathf.Floor(position.x / (GameConfig.ROOM_WIDTH * GameConfig.BLOCK_SIZE));
        int coordZ = (int)Mathf.Floor(position.z / (GameConfig.ROOM_HEIGHT * GameConfig.BLOCK_SIZE));
        return new XY(coordX, coordZ);
    }

    private void DrawMap() {
        foreach(MapNode node in MapModel.Graph.BreadthFirstSearch(MapModel.Entrance))
            DrawRoom(node);
    }

    private void DrawRoom(MapNode node) {
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

                GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);
                block.transform.position = new Vector3(blockX, 0, blockZ);
                block.transform.localScale = new Vector3(blockSize - 1, blockSize / 2, blockSize - 1);
                block.transform.parent = mapRoomEntity.transform;

                if(y == 0) {
                    if(x < 6 || x > 9 || !node.Edges.ContainsKey(GridDirection.S))
                        DrawWall(blockX, blockZ, mapRoomEntity.transform);
                } else if(y == roomHeight - 1) {
                    if(x < 6 || x > 9 || !node.Edges.ContainsKey(GridDirection.N))
                        DrawWall(blockX, blockZ, mapRoomEntity.transform);
                } else if(x == 0) {
                    if(y < 4 || y > 7 || !node.Edges.ContainsKey(GridDirection.W))
                        DrawWall(blockX, blockZ, mapRoomEntity.transform);
                } else if(x == roomWidth - 1) {
                    if(y < 4 || y > 7 || !node.Edges.ContainsKey(GridDirection.E))
                        DrawWall(blockX, blockZ, mapRoomEntity.transform);
                }
            }
        }
    }

    private void DrawWall(int x, int z, Transform parent) {
        int blockSize = GameConfig.BLOCK_SIZE;

        for(int i = 0; i < 4; i++) {
            GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);
            block.transform.position = new Vector3(x, i * blockSize, z);
            block.transform.localScale = new Vector3(blockSize - 1, blockSize - 1, blockSize - 1);
            block.transform.parent = parent;
        }
    }
}
