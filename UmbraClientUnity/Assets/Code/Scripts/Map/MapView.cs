using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using DungeonNode = GridVertex<DungeonRoom, DungeonPath>;

public class MapView : MonoBehaviour {
    public Rect RoomBounds { get; private set; }

    private Dungeon _dungeon;
    private GameObject _dungeonView;

    public void SetDungeon(Dungeon dungeon) {
        _dungeon = dungeon;

        DrawDungeon();
    }

    public void UpdateRoomBounds(XY coord) {
        int blockSize = GameConfig.BLOCK_SIZE;

        float roomWidth = (GameConfig.ROOM_WIDTH * blockSize);
        float roomHeight = (GameConfig.ROOM_HEIGHT * blockSize);

        float left = coord.X * roomWidth - (blockSize / 2);
        float top = coord.Y * roomHeight - (blockSize / 2);

        RoomBounds = new Rect(left, top, roomWidth, roomHeight);
    }

    private void DrawDungeon() {
        _dungeonView = new GameObject("Dungeon");

        foreach(DungeonNode node in _dungeon.Graph.BreadthFirstSearch(_dungeon.Entrance))
            DrawRoom(node);
    }

    private void DrawRoom(DungeonNode node) {
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
                block.transform.parent = _dungeonView.transform;

                if(y == 0) {
                    if(x < 6 || x > 9 || !node.Edges.ContainsKey(GridDirection.S))
                        DrawWall(blockX, blockZ);
                } else if(y == roomHeight - 1) {
                    if(x < 6 || x > 9 || !node.Edges.ContainsKey(GridDirection.N))
                        DrawWall(blockX, blockZ);
                } else if(x == 0) {
                    if(y < 4 || y > 7 || !node.Edges.ContainsKey(GridDirection.W))
                        DrawWall(blockX, blockZ);
                } else if(x == roomWidth - 1) {
                    if(y < 4 || y > 7 || !node.Edges.ContainsKey(GridDirection.E))
                        DrawWall(blockX, blockZ);
                }
            }
        }
    }

    private void DrawWall(int x, int z) {
        int blockSize = GameConfig.BLOCK_SIZE;

        for(int i = 0; i < 4; i++) {
            GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);
            block.transform.position = new Vector3(x, i * blockSize, z);
            block.transform.localScale = new Vector3(blockSize - 1, blockSize - 1, blockSize - 1);
            block.transform.parent = _dungeonView.transform;
        }
    }
}
