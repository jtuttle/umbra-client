using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum GameState {
    MapEnter, MapWalk, MapDesign, PlayerPlace
}

public class GameManager : UnitySingleton<GameManager> {
    public Camera GameCamera;

    public Map CurrentMap { get; private set; }
    public XY CurrentCoord { get; private set; }

    private InputManager _inputManager;
    public InputManager Input { get { return _inputManager; } }

    // useful references to have available, may be a better place for these
    public GameObject Player;
    public GameObject Map;

    private FiniteStateMachine _fsm;

    public override void Awake() {
        _inputManager = GetComponent<InputManager>();

        _fsm = new FiniteStateMachine();
        _fsm.AddState(new MapEnterState());
        _fsm.AddState(new MapWalkState());
        _fsm.AddState(new MapDesignState());
        _fsm.AddState(new PlayerPlaceState());
    }

    public void Start() {
        CurrentMap = new MapGenerator().Generate(10, 10);
        CurrentCoord = CurrentMap.Entrance.Coord;

        _fsm.ChangeState(new FSMTransition(GameState.MapEnter));
    }

    public void Update() {
        _fsm.Update();
    }

    public void UpdateCurrentCoord(Vector3 from, Vector3 to) {
        int dx = to.x < from.x ? -1 : (to.x > from.x ? 1 : 0);
        int dy = to.z < from.z ? -1 : (to.z > from.z ? 1 : 0);
        CurrentCoord = CurrentCoord + new XY(dx, dy);
    }
}
