using UnityEngine;
using System.Collections;

public class MapDesignState : BaseGameState {
    public PlayerView PlayerView { get; private set; }

    private MapViewCamera _mapViewCamera;

    public MapDesignState(PlayerView playerView)
        : base(GameStates.MapDesign) {

        PlayerView = playerView;
    }

    public override void EnterState() {
        base.EnterState();

        _mapViewCamera = GameManager.Instance.GameCamera.GetComponent<MapViewCamera>();

        // set up camera transition response
        _mapViewCamera.OnMoveBegin += OnCameraMoveBegin;
        _mapViewCamera.OnMoveEnd += OnCameraMoveEnd;

        //PlayerView = GameObject.FindObjectOfType(typeof(PlayerView)) as PlayerView;
        PlayerView.Freeze();
        PlayerView.gameObject.SetActive(false);

        AddPlayerInput();
    }

    public override void ExitState() {
        // set up camera transition response
        _mapViewCamera.OnMoveBegin -= OnCameraMoveBegin;
        _mapViewCamera.OnMoveEnd -= OnCameraMoveEnd;

        RemovePlayerInput();

        base.ExitState();
    }

    public override void Dispose() {
        base.Dispose();

        PlayerView = null;
        _mapViewCamera = null;
    }

    private void AddPlayerInput() {
        InputManager input = GameManager.Instance.Input;
        input.OnAxialInput += OnAxialInput;
        input.OnSpecialPress += OnSpecialPress;
    }

    private void RemovePlayerInput() {
        InputManager input = GameManager.Instance.Input;
        input.OnAxialInput -= OnAxialInput;
        input.OnSpecialPress -= OnSpecialPress;
    }

    private void OnCameraMoveBegin(XY delta) {
        RemovePlayerInput();
    }

    private void OnCameraMoveEnd(XY delta) {
        AddPlayerInput();
    }

    // TODO: make this smarter
    private void OnAxialInput(float h, float v) {
        if(_mapViewCamera.Moving) return;

        XY fullScreen = _mapViewCamera.FullScreen;

        XY delta = null;

        if(h < 0) {
            delta = new XY(-fullScreen.X, 0);
        } else if(h > 0) {
            delta = new XY(fullScreen.X, 0);
        } else if(v < 0) {
            delta = new XY(0, -fullScreen.Y);
        } else if(v > 0) {
            delta = new XY(0, fullScreen.Y);
        }

        if(delta != null)
            _mapViewCamera.Move(delta);
    }

    private void OnSpecialPress() {
        NextState = GameStates.MapWalk;

        // TODO: allow player to place themselves in a new state eventually, for now just center on current screen
        Vector3 camPos = _mapViewCamera.transform.position;
        XY halfScreen = _mapViewCamera.HalfScreen;
        PlayerView.transform.position = new Vector3(camPos.x + halfScreen.X, camPos.y + halfScreen.Y, 0);

        ExitState();
    }
}
