using System.Collections;
using uLink;
using UnityEngine;

public class NetworkServer : UnityEngine.MonoBehaviour {
    public int MaxConnections = 2;
    public int Port = 7100;
    public int LevelSeed = 1337;

    public GameObject PlayerCreator;
    public GameObject PlayerOwner;
    public GameObject PlayerProxy;

    private MapEntity _mapEntity;

    protected void Awake() {
        uLink.Network.isAuthoritativeServer = true;
        uLink.Network.InitializeServer(MaxConnections, Port);
    }

    protected void uLink_OnServerInitialized() {
        Debug.Log("Server started successfully");

        Random.seed = LevelSeed;

        LoadMap();
    }

    protected void uLink_OnPlayerApproval(NetworkPlayerApproval approval) {
        approval.Approve(LevelSeed);
    }

    protected void uLink_OnPlayerConnected(uLink.NetworkPlayer player) {
        Debug.Log("Player connected from " + player.ipAddress + ":" + player.port);

        InstantiatePlayer(player);
    }

    protected void uLink_OnPlayerDisconnected(uLink.NetworkPlayer player) {
        uLink.Network.DestroyPlayerObjects(player);
        uLink.Network.RemoveRPCs(player);
    }

    private void LoadMap() {
        Map map = new MapGenerator().Generate(10, 10);

        GameObject mapGo = UnityUtils.LoadResource<GameObject>("Prefabs/Map", true);
        mapGo.name = "Map";

        _mapEntity = mapGo.GetComponent<MapEntity>();
        _mapEntity.SetMap(map);
    }

    private void InstantiatePlayer(uLink.NetworkPlayer player) {
        Rect roomBounds = _mapEntity.GetBoundsForCoord(new XY(0, 0));

        Vector2 mapCenter = roomBounds.center;
        Vector3 startPos = new Vector3(mapCenter.x, GameConfig.BLOCK_SIZE, mapCenter.y);

        uLink.Network.Instantiate(player, PlayerProxy, PlayerOwner, PlayerCreator, startPos, Quaternion.identity, 0, "Villian");
    }
}
