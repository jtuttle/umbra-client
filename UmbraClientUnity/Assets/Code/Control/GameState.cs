using System.Collections;

public enum GameStates {
    Loading, MainMenu, MapEnter, MapWalk, MapExit, Shop, Design, MiniMap
}

public class GameState {
    public delegate void StateChangeDelegate(BaseGameState state);
    public event StateChangeDelegate OnStateExit = delegate { };

    public BaseGameState CurrentState { get; private set; }

    private Stack _stateStack;

    public GameState() {
        _stateStack = new Stack();
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
