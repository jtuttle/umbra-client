using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameManager : UnitySingleton<GameManager> {
    private GameStateMachine _states;

    public Camera GameCamera;

    public Map CurrentMap { get; private set; }
    public XY CurrentCoord { get; private set; }

    private InputManager _inputManager;
    public InputManager Input { get { return _inputManager; } }

    // useful references to have available, may be a better place for these
    public GameObject Player;
    public GameObject Map;

    public override void Awake() {
        _states = new GameStateMachine();

        _inputManager = GetComponent<InputManager>();
    }

    public void Start() {
        _states.OnStateExit += OnExitState;

        CurrentMap = new MapGenerator().Generate(10, 10);
        CurrentCoord = CurrentMap.Entrance.Coord;
        
        _states.ChangeGameState(new MapEnterState(CurrentMap));
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
                _states.ChangeGameState(new MapWalkState());

                break;
            case GameStates.MapWalk:

                if(state.NextState == GameStates.MapDesign)
                    _states.ChangeGameState(new MapDesignState(), true);

                break;
            case GameStates.MapDesign:
                //_states.RestorePreviousState();

                if(state.NextState == GameStates.PlayerPlace)
                    _states.ChangeGameState(new PlayerPlaceState(), true);

                break;
            case GameStates.PlayerPlace:
                _states.RestorePreviousState(state.NextState);

                break;
            default:
                throw new Exception("Game state not found: " + state.GameState.ToString());
        }
    }
}
