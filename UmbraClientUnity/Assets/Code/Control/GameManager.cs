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
        //CurrentDungeon = new Dungeon(MiniJSON.jsonDecode("{\"Nodes\":[{\"Coord\":{\"X\":0, \"Y\":0}, \"Room\":{}}, {\"Coord\":{\"X\":0, \"Y\":1}, \"Room\":{}}, {\"Coord\":{\"X\":0, \"Y\":-1}, \"Room\":{}}, {\"Coord\":{\"X\":-1, \"Y\":0}, \"Room\":{}}, {\"Coord\":{\"X\":1, \"Y\":1}, \"Room\":{}}, {\"Coord\":{\"X\":-1, \"Y\":1}, \"Room\":{}}, {\"Coord\":{\"X\":-1, \"Y\":-1}, \"Room\":{}}, {\"Coord\":{\"X\":-2, \"Y\":0}, \"Room\":{}}, {\"Coord\":{\"X\":-1, \"Y\":2}, \"Room\":{}}, {\"Coord\":{\"X\":-2, \"Y\":1}, \"Room\":{}}], \"Edges\":[{\"From\":{\"X\":0, \"Y\":0}, \"Path\":{}, \"To\":{\"X\":-1, \"Y\":0}}, {\"From\":{\"X\":0, \"Y\":0}, \"Path\":{}, \"To\":{\"X\":-1, \"Y\":0}}, {\"From\":{\"X\":0, \"Y\":0}, \"Path\":{}, \"To\":{\"X\":-1, \"Y\":0}}, {\"From\":{\"X\":0, \"Y\":1}, \"Path\":{}, \"To\":{\"X\":1, \"Y\":1}}, {\"From\":{\"X\":0, \"Y\":1}, \"Path\":{}, \"To\":{\"X\":1, \"Y\":1}}, {\"From\":{\"X\":0, \"Y\":1}, \"Path\":{}, \"To\":{\"X\":1, \"Y\":1}}, {\"From\":{\"X\":0, \"Y\":-1}, \"Path\":{}, \"To\":{\"X\":0, \"Y\":0}}, {\"From\":{\"X\":-1, \"Y\":0}, \"Path\":{}, \"To\":{\"X\":-2, \"Y\":0}}, {\"From\":{\"X\":-1, \"Y\":0}, \"Path\":{}, \"To\":{\"X\":-2, \"Y\":0}}, {\"From\":{\"X\":-1, \"Y\":0}, \"Path\":{}, \"To\":{\"X\":-2, \"Y\":0}}, {\"From\":{\"X\":1, \"Y\":1}, \"Path\":{}, \"To\":{\"X\":0, \"Y\":1}}, {\"From\":{\"X\":-1, \"Y\":1}, \"Path\":{}, \"To\":{\"X\":-2, \"Y\":1}}, {\"From\":{\"X\":-1, \"Y\":1}, \"Path\":{}, \"To\":{\"X\":-2, \"Y\":1}}, {\"From\":{\"X\":-1, \"Y\":1}, \"Path\":{}, \"To\":{\"X\":-2, \"Y\":1}}, {\"From\":{\"X\":-1, \"Y\":-1}, \"Path\":{}, \"To\":{\"X\":-1, \"Y\":0}}, {\"From\":{\"X\":-2, \"Y\":0}, \"Path\":{}, \"To\":{\"X\":-1, \"Y\":0}}, {\"From\":{\"X\":-1, \"Y\":2}, \"Path\":{}, \"To\":{\"X\":-1, \"Y\":1}}, {\"From\":{\"X\":-2, \"Y\":1}, \"Path\":{}, \"To\":{\"X\":-1, \"Y\":1}}], \"Entrance\":{\"X\":0, \"Y\":0}}") as Hashtable);

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
