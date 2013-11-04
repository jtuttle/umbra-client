using System.Collections.Generic;
using UnityEngine;

public enum GameStates {
    Loading, MainMenu,
    Shop, 
    MapEnter, MapWalk, MapDesign, MapExit, 
    ObjectPlace,
    MiniMap
}

public class GameStateMachine {
    public delegate void StateChangeDelegate(BaseGameState state);
    public event StateChangeDelegate OnStateExit = delegate { };

    public BaseGameState CurrentState { get; private set; }

    public BaseGameState PreviousState {
        get { return _stateStack.Peek(); }
    }

    private Stack<BaseGameState> _stateStack;

    public GameStateMachine() {
        _stateStack = new Stack<BaseGameState>();
    }

    public void ChangeGameState(BaseGameState newState, bool pushPreviousState = false) {
        // destroy or push old state
        if(CurrentState != null) {
            CurrentState.OnExit -= OnExit;

            if(pushPreviousState)
                _stateStack.Push(CurrentState);
            else
                CurrentState.Dispose();
        }

        // set new state
        CurrentState = newState;

        Debug.Log("Changed state to " + CurrentState.GameState.ToString());

        CurrentState.OnExit += OnExit;
        CurrentState.EnterState();
    }

    public void RestorePreviousState() {
        if(CurrentState != null) {
            CurrentState.OnExit -= OnExit;
            CurrentState.Dispose();
        }

        CurrentState = (BaseGameState)_stateStack.Pop();

        CurrentState.OnExit += OnExit;
        CurrentState.EnterState();
    }

    private void OnExit(BaseGameState exitingState) {
        OnStateExit(exitingState);
    }
}
