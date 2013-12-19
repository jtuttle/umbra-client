using UnityEngine;
using System.Collections;

public class MapDesignState : BaseGameState {
    private GameObject _player;
    private MapEntity _map;
    private TweenMover _cameraMover;

    public MapDesignState()
        : base(GameStates.MapDesign) {

    }

    public override void EnterState() {
        base.EnterState();

        _player = GameManager.Instance.Player;
        _player.gameObject.SetActive(false);

        _map = GameManager.Instance.MapView;
        _cameraMover = GameManager.Instance.GameCamera.GetComponent<TweenMover>();

        // set up camera transition response
        _cameraMover.OnMoveBegin += OnCameraMoveBegin;
        _cameraMover.OnMoveEnd += OnCameraMoveEnd;

        EnableInput();
    }

    public override void ExitState() {
        // set up camera transition response
        _cameraMover.OnMoveBegin -= OnCameraMoveBegin;
        _cameraMover.OnMoveEnd -= OnCameraMoveEnd;

        DisableInput();

        base.ExitState();
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
        _map.UpdateRoomBounds(GameManager.Instance.CurrentCoord);

        EnableInput();
    }

    // TODO: make this smarter
    private void OnAxialInput(float h, float v) {
        if(_cameraMover.Moving) return;

        Rect roomBounds = _map.RoomBounds;

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
        NextState = GameStates.PlayerPlace;

        ExitState();
    }
}
