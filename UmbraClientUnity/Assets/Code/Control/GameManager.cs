using UnityEngine;
using System.Collections;
using System;

public class GameManager : UnitySingleton<GameManager> {
    private GameStateMachine _states;

    public Camera GameCamera;

    public Dungeon CurrentDungeon { get; private set; }

    private InputManager _inputManager;
    public InputManager Input { get { return _inputManager; } }

    public override void Awake() {
        _states = new GameStateMachine();

        _inputManager = GetComponent<InputManager>();
    }

    void Start() {
        _states.OnStateExit += OnExitState;

        CurrentDungeon = new DungeonGenerator().Generate(10);

        // grab appropriate tileset from somewhere...
        tk2dSpriteCollectionData tileset = UnityUtils.LoadResource<tk2dSpriteCollectionData>("SpriteCollectionData/TestTileSet");

        _states.ChangeGameState(new MapEnterState(tileset, 64));
    }

    void Update() {
        if(_states.CurrentState != null)
            _states.CurrentState.Update();
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
