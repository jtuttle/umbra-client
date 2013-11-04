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

    public override void Awake() {
        _states = new GameStateMachine();

        _inputManager = GetComponent<InputManager>();
    }

    public void Start() {
        _states.OnStateExit += OnExitState;

        CurrentMap = new MapGenerator().Generate(10);
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
                Vector3 initialPlayerPosition = (state as MapEnterState).PlayerPosition;
                _states.ChangeGameState(new MapWalkState(initialPlayerPosition));

                break;
            case GameStates.MapWalk:

                if(state.NextState == GameStates.MapDesign) {
                    MapDesignState mapDesignState = new MapDesignState();
                    _states.ChangeGameState(mapDesignState, true);
                }

                break;
            case GameStates.MapDesign:
                //_states.RestorePreviousState();

                if(state.NextState == GameStates.ObjectPlace) {
                    List<GameObject> options = new List<GameObject>() { UnityUtils.LoadResource<GameObject>("Prefabs/Player") };
                    ObjectPlaceState objectPlaceState = new ObjectPlaceState(options);
                    _states.ChangeGameState(objectPlaceState);
                }

                break;
            case GameStates.ObjectPlace:
                MapWalkState walkState = _states.PreviousState as MapWalkState;
                walkState.UpdatePlayerPosition((state as ObjectPlaceState).Placement);

                _states.RestorePreviousState();

                break;
            default:
                throw new Exception("Game state not found: " + state.GameState.ToString());
        }
    }
}
