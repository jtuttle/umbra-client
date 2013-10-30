using UnityEngine;
using System.Collections;

using DungeonNode = GridVertex<DungeonRoom, DungeonPath>;
using Holoville.HOTween;

public class PrototypeScene : MonoBehaviour {
    private readonly int ROOM_WIDTH = 16;
    private readonly int ROOM_HEIGHT = 12;
    private readonly int BLOCK_SIZE = 16;

    private Dungeon _dungeon;
    private GameObject _dungeonGo;
    private PlayerView _player;

    private DungeonNode _currentRoom;

    private bool _camMoving = false;

	void Awake () {
        _dungeon = new DungeonGenerator().Generate(10);

        DrawDungeon(_dungeon);

        _currentRoom = _dungeon.Entrance;

        SetCamera();

        AddPlayer();
        AddPlayerInput();
	}

    private Rect GetRoomBounds() {
        XY coord = _currentRoom.Coord;

        float roomWidth = (ROOM_WIDTH * BLOCK_SIZE);
        float roomHeight = (ROOM_HEIGHT * BLOCK_SIZE);

        float left = coord.X * roomWidth - (BLOCK_SIZE / 2);
        float top = coord.Y * roomHeight - (BLOCK_SIZE / 2);

        return new Rect(left, top, roomWidth, roomHeight);
    }

    private void Update() {
        // can use this to make camera always point at center of room, good for moving 
        // cam around in editor window to determine a nice-looking y/z coordinate 
        // pair, might also want to try pointing camera at different part of room
        //Camera cam = Camera.main;
        //float lookX = -(BLOCK_SIZE / 2) + (ROOM_WIDTH * BLOCK_SIZE) / 2;
        //float lookZ = -(BLOCK_SIZE / 2) + (ROOM_HEIGHT * BLOCK_SIZE) / 2;
        //cam.transform.LookAt(new Vector3(lookX, 0, lookZ));

        if(!_camMoving) {
            Vector3 playerPos = _player.transform.position;
            Rect roomBounds = GetRoomBounds();

            //Debug.Log(roomBounds.xMin + " " + roomBounds.xMax + " " + roomBounds.yMin + " " + roomBounds.yMax);

            if(playerPos.x < roomBounds.xMin) {
                MoveCamera(new XY((int)-roomBounds.width, 0));
            } else if(playerPos.x > roomBounds.xMax) {
                MoveCamera(new XY((int)roomBounds.width, 0));
            } else if(playerPos.z < roomBounds.yMin) {
                MoveCamera(new XY(0, -(int)roomBounds.height));
            } else if(playerPos.z > roomBounds.yMax) {
                MoveCamera(new XY(0, (int)roomBounds.height));
            }
        }
    }

    private void MoveCamera(XY delta) {
        _camMoving = true;
        _player.Freeze();
        RemovePlayerInput();

        Vector3 goPos = Camera.main.transform.position;
        Vector3 newPos = goPos + new Vector3(delta.X, 0, delta.Y);

        //OnMoveBegin(delta);

        TweenParms parms = new TweenParms();
        parms.Ease(EaseType.Linear);
        parms.Prop("position", newPos);
        parms.OnComplete(OnMoveComplete, delta);

        //Moving = true;

        HOTween.To(Camera.main.transform, 0.5f, parms);
    }

    private void OnMoveComplete(TweenEvent e) {
        XY delta = (XY)e.parms[0];

        int dx = delta.X > 0 ? 1 : (delta.X < 0 ? -1 : 0);
        int dy = delta.Y > 0 ? 1 : (delta.Y < 0 ? -1 : 0);

        _currentRoom = _dungeon.Graph.GetVertexByCoord(new XY(_currentRoom.Coord.X + dx, _currentRoom.Coord.Y + dy));

        _camMoving = false;
        _player.Unfreeze();
        AddPlayerInput();

        //XY delta = (XY)e.parms[0];

        //OnMoveEnd(delta);
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

    private void AddPlayer() {
        _player = UnityUtils.LoadResource<GameObject>("Prefabs/Player", true).GetComponent<PlayerView>();

        float playerX = -(BLOCK_SIZE / 2) + (ROOM_WIDTH * BLOCK_SIZE) / 2;
        float playerZ = -(BLOCK_SIZE / 2) + (ROOM_HEIGHT * BLOCK_SIZE) / 2;
        float playerY = BLOCK_SIZE;

        _player.transform.position = new Vector3(playerX, playerY, playerZ);
        _player.renderer.material.color = Color.blue;
    }

    private void AddPlayerInput() {
        GameObject.Find("InputManager").GetComponent<InputManager>().OnAxialInput += _player.Move;
    }

    private void RemovePlayerInput() {
        GameObject.Find("InputManager").GetComponent<InputManager>().OnAxialInput -= _player.Move;
    }

    private void DrawDungeon(Dungeon dungeon) {
        _dungeonGo = new GameObject("Dungeon");

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
                block.transform.parent = _dungeonGo.transform;

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
            block.transform.parent = _dungeonGo.transform;
        }
    }
}
