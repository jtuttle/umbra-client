using UnityEngine;
using System.Collections;

public class NetworkClient : UnityEngine.MonoBehaviour {
    public string ServerIp = "127.0.0.1";
    public int ServerPort = 7100;

    protected void Awake() {
        uLink.Network.isAuthoritativeServer = true;
        uLink.Network.Connect(ServerIp, ServerPort);
    }

    protected void uLink_OnConnectedToServer() {
        Debug.Log("Connected to server on port: " + uLink.Network.player.port.ToString());

        int levelSeed = (int)uLink.Network.approvalData.ReadObject(typeof(int).TypeHandle);
        Debug.Log("Retrieved level seed: " + levelSeed.ToString());

        Random.seed = levelSeed;

        SetCamera();
        LoadMap();
    }

    protected void uLink_OnFailedToConnect(uLink.NetworkConnectionError error) {
        Debug.Log("Failed to connect: " + error);
    }

    private void SetCamera() {
        int blockSize = GameConfig.BLOCK_SIZE;

        Camera cam = Camera.main;

        float camX = -(blockSize / 2) + (GameConfig.ROOM_WIDTH * blockSize) / 2;
        float camY = 180.0f;
        float camZ = 10.0f;

        float lookX = camX;
        float lookZ = -(blockSize / 2) + (GameConfig.ROOM_HEIGHT * blockSize) / 2;

        cam.transform.position = new Vector3(camX, camY, camZ);
        cam.transform.LookAt(new Vector3(lookX, 0, lookZ));
    }

    private void LoadMap() {
        Map map = new MapGenerator().Generate(10, 10);

        GameObject mapGo = UnityUtils.LoadResource<GameObject>("Prefabs/Map", true);
        mapGo.name = "Map";

        mapGo.GetComponent<MapEntity>().SetMap(map);
    }
}
