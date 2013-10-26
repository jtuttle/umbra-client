using UnityEngine;
using System.Collections;

using DungeonNode = GridVertex<DungeonRoom, DungeonPath>;

public class PrototypeScene : MonoBehaviour {
    private readonly int ROOM_WIDTH = 16;
    private readonly int ROOM_HEIGHT = 12;
    private readonly int BLOCK_SIZE = 16;

    private GameObject _dungeon;

	void Awake () {
        Dungeon dungeon = new DungeonGenerator().Generate(10);

        DrawDungeon(dungeon);

        SetCamera();
	}

    private void Update() {
        // can use this to make camera always point at center of room, good for moving 
        // cam around in editor window to determine a nice-looking y/z coordinate 
        // pair, might also want to try pointing camera at different part of room
        //Camera cam = Camera.main;
        //float lookX = -(BLOCK_SIZE / 2) + (ROOM_WIDTH * BLOCK_SIZE) / 2;
        //float lookZ = -(BLOCK_SIZE / 2) + (ROOM_HEIGHT * BLOCK_SIZE) / 2;
        //cam.transform.LookAt(new Vector3(lookX, 0, lookZ));
    }

    private void SetCamera() {
        Camera cam = Camera.main;

        float camX = -(BLOCK_SIZE / 2) + (ROOM_WIDTH * BLOCK_SIZE) / 2;
        float camY = 180.0f;
        float camZ = 10.0f;

        float lookX = camX;
        float lookZ = -(BLOCK_SIZE / 2) + (ROOM_HEIGHT * BLOCK_SIZE) / 2;

        cam.transform.position = new Vector3(camX, camY, camZ);
        cam.transform.LookAt(new Vector3(lookX, 0, lookZ));
    }

    private void DrawDungeon(Dungeon dungeon) {
        _dungeon = new GameObject("Dungeon");

        foreach(DungeonNode node in dungeon.Graph.BreadthFirstSearch(dungeon.Entrance)) {
            DrawRoom(node);
        }
    }

    private void DrawRoom(DungeonNode node) {
        XY start = new XY(node.Coord.X * ROOM_WIDTH * BLOCK_SIZE, node.Coord.Y * ROOM_HEIGHT * BLOCK_SIZE);

        for(int y = 0; y < ROOM_HEIGHT; y++) {
            for(int x = 0; x < ROOM_WIDTH; x++) {
                int blockX = start.X + x * BLOCK_SIZE;
                int blockZ = start.Y + y * BLOCK_SIZE;

                GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);
                block.transform.position = new Vector3(blockX, 0, blockZ);
                block.transform.localScale = new Vector3(BLOCK_SIZE - 1, BLOCK_SIZE / 2, BLOCK_SIZE - 1);
                block.transform.parent = _dungeon.transform;

                if(y == 0) {
                    if(x < 6 || x > 9 || !node.Edges.ContainsKey(GridDirection.S))
                        DrawWall(blockX, blockZ);
                } else if(y == ROOM_HEIGHT - 1) {
                    if(x < 6 || x > 9 || !node.Edges.ContainsKey(GridDirection.N))
                        DrawWall(blockX, blockZ);
                } else if(x == 0) {
                    if(y < 4 || y > 7 || !node.Edges.ContainsKey(GridDirection.W))
                        DrawWall(blockX, blockZ);
                } else if(x == ROOM_WIDTH - 1) {
                    if(y < 4 || y > 7 || !node.Edges.ContainsKey(GridDirection.E))
                        DrawWall(blockX, blockZ);
                }
            }
        }
    }

    private void DrawWall(int x, int z) {
        for(int i = 0; i < 4; i++) {
            GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);
            block.transform.position = new Vector3(x, i * BLOCK_SIZE, z);
            block.transform.localScale = new Vector3(BLOCK_SIZE - 1, BLOCK_SIZE - 1, BLOCK_SIZE - 1);
            block.transform.parent = _dungeon.transform;
        }
    }
}
