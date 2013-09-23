using UnityEngine;
using System.Collections;
using System;

public class GameManager : UnitySingleton<GameManager> {
    private GameState _states;

    public Camera GameCamera;

    public Map CurrentMap { get; private set; }

    public override void Awake() {
        _states = new GameState();
    }

    void Start() {
        _states.OnStateExit += OnExitState;

        // grab this map from somewhere...
        Map map = Map.CreateFake(48, 24);

        // grab appropriate tileset from somewhere...
        tk2dSpriteCollectionData tileset = UnityUtils.LoadResource<tk2dSpriteCollectionData>("SpriteCollectionData/TestTileSet");

        _states.ChangeGameState(new GameEnterState(map, tileset, 64));
    }

    void Update() {
        if(_states.CurrentState != null)
            _states.CurrentState.Update();
    }

    private void OnExitState(BaseGameState state) {
        switch(state.GameState) {
            case GameStates.GameEnter:

                break;
            default:
                throw new Exception("Game state not found: " + state.GameState.ToString());
        }
    }
}
