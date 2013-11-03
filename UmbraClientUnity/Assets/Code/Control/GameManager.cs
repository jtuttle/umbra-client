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
	
	public UmbraApi Api { get; private set; }
	public UmbraClient Client { get; private set; }

    new public void Awake() {
        _states = new GameStateMachine();

        _inputManager = GetComponent<InputManager>();
		
		Api = new UmbraApi("localhost", 3000);
		
		/* TODO RT: this wont work long run, since you have to use the API to
		 * join an "island", so you cant connect until doing that whole negotiation
		 * but this will work for now... */
		string authkey = Api.DoSignIn("rtortora@craw.cc", "fish");
		Client = new UmbraClient("localhost", 9000);
		Client.Start();
		Client.SendAuth(authkey, 0, 0);
    }

    public void Start() {
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
