using UnityEngine;
using System.Collections;
using System;
using ClientLib;

public class GameManager : UnitySingleton<GameManager> {
    private GameStateMachine _states;

    public Camera GameCamera;

    public Dungeon CurrentDungeon { get; private set; }
    public XY CurrentCoord { get; private set; }

    private InputManager _inputManager;
    public InputManager Input { get { return _inputManager; } }

    public MpApi Api { get; private set; }
    public MpClient Client { get; private set; }
    public string AuthKey { get; private set; }

    public override void Awake() {
        _states = new GameStateMachine();

        _inputManager = GetComponent<InputManager>();

        // TODO RT: move this to a config
        string host = "10.0.0.4";

        Api = new MpApi(host, 3000);

        /* TODO RT: this wont work long run, since you have to use the API to
         * join an "island", so you cant connect until doing that whole negotiation
         * but this will work for now... */
        AuthKey = Api.DoSignIn("rtortora@craw.cc", "fish");
        MpUtil.Log("Auth key: " + AuthKey);
        Client = new MpClient(host, 9000);
    }

    public void Start() {
        Client.Start(); // TODO RT: we need a place to call Stop so we can terminate the thread
        Client.SendAuth(AuthKey, 0, 0, 0);
        MpUtil.Log("Sent Auth");

        _states.OnStateExit += OnExitState;

        CurrentDungeon = new DungeonGenerator().Generate(10);
        CurrentCoord = CurrentDungeon.Entrance.Coord;
        
        _states.ChangeGameState(new MapEnterState(CurrentDungeon));
    }

    public void Update() {
        if(_states.CurrentState != null)
            _states.CurrentState.Update();
    }

    public void UpdateCurrentCoord(Vector3 from, Vector3 to) {
        int dx = to.x < from.x ? -1 : (to.x > from.x ? 1 : 0);
        int dy = to.z < from.z ? -1 : (to.z > from.z ? 1 : 0);
        CurrentCoord = CurrentCoord + new XY(dx, dy);
    }

    private void OnExitState(BaseGameState state) {
        switch(state.GameState) {
            case GameStates.MapEnter:
                PlayerView playerView = (state as MapEnterState).PlayerView;
                _states.ChangeGameState(new MapWalkState(playerView));

                break;
            case GameStates.MapWalk:

                if(state.NextState == GameStates.MapDesign) {
                    MapDesignState mapDesignState = new MapDesignState((state as MapWalkState).PlayerView);
                    _states.ChangeGameState(mapDesignState, true);
                }

                break;
            case GameStates.MapDesign:
                _states.RestorePreviousState();

                break;
            default:
                throw new Exception("Game state not found: " + state.GameState.ToString());
        }
    }
}
