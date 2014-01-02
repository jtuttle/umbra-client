using UnityEngine;
using System.Collections;

public class MapDesignState : FSMState {
    private GameObject _player;
    private MapEntity _mapEntity;
    private TweenMover _cameraMover;

    public MapDesignState()
        : base(GameState.MapDesign) {

    }

    public override void EnterState(FSMState prevState) {
        base.EnterState(prevState);

        _player = GameManager.Instance.Player;
        _player.gameObject.SetActive(false);

        _mapEntity = GameManager.Instance.Map.GetComponent<MapEntity>();
        _cameraMover = GameManager.Instance.GameCamera.GetComponent<TweenMover>();

        // set up camera transition response
        _cameraMover.OnMoveBegin += OnCameraMoveBegin;
        _cameraMover.OnMoveEnd += OnCameraMoveEnd;

        EnableInput();
    }

    public override void ExitState(FSMTransition nextStateTransition) {
        // set up camera transition response
        _cameraMover.OnMoveBegin -= OnCameraMoveBegin;
        _cameraMover.OnMoveEnd -= OnCameraMoveEnd;

        DisableInput();

        base.ExitState(nextStateTransition);
    }

    public override void Dispose() {
        base.Dispose();

        _cameraMover = null;
    }

    private void EnableInput() {
        InputManager input = GameManager.Instance.Input;
        input.OnAxialInput += OnAxialInput;
        input.GetButton(ButtonId.Special).OnPress += OnSpecialPress;
    }

    private void DisableInput() {
        InputManager input = GameManager.Instance.Input;
        input.OnAxialInput -= OnAxialInput;
        input.GetButton(ButtonId.Special).OnPress -= OnSpecialPress;
    }

    private void OnCameraMoveBegin(Vector3 from, Vector3 to) {
        DisableInput();
    }

    private void OnCameraMoveEnd(Vector3 from, Vector3 to) {
        GameManager.Instance.UpdateCurrentCoord(from, to);
        
        EnableInput();
    }

    // TODO: make this smarter
    private void OnAxialInput(float h, float v) {
        if(_cameraMover.Moving) return;

        Rect roomBounds = _mapEntity.GetBoundsForCoord(GameManager.Instance.CurrentCoord);

        XY delta = null;

        if(h < 0)
            delta = new XY((int)-roomBounds.width, 0);
        else if(h > 0)
            delta = new XY((int)roomBounds.width, 0);
        else if(v < 0)
            delta = new XY(0, -(int)roomBounds.height);
        else if(v > 0)
            delta = new XY(0, (int)roomBounds.height);

        if(delta != null)
            _cameraMover.Move(delta);
    }

    private void OnSpecialPress() {
        ExitState(new FSMTransition(GameState.PlayerPlace));
    }
}
