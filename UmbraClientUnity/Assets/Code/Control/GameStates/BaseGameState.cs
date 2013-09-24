using UnityEngine;
using System.Collections;

public abstract class BaseGameState : ITurnState {
    public delegate void StateChangeDelegate(BaseGameState state);

    public event StateChangeDelegate OnEnter = delegate { };
    public event StateChangeDelegate OnExit = delegate { };

    public GameStates GameState { get; private set; }

    public BaseGameState(GameStates gameState) {
        GameState = gameState;
    }

    public virtual void EnterState() {
        OnEnter(this);
    }

    public virtual void ExitState() {
        OnExit(this);
    }

    public virtual void Update() { }

    public virtual void Dispose() { }
}