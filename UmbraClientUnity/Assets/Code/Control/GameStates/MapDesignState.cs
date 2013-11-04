using UnityEngine;
using System.Collections;

public class MapDesignState : BaseGameState {
    public PlayerView PlayerView { get; private set; }

    private MapView _mapView;
    private MapViewCamera _mapViewCamera;

    public MapDesignState()
        : base(GameStates.MapDesign) {

    }

    public override void EnterState() {
        base.EnterState();

        _mapView = GameObject.Find("MapView").GetComponent<MapView>();
        _mapViewCamera = GameManager.Instance.GameCamera.GetComponent<MapViewCamera>();
        
        // set up camera transition response
        _mapViewCamera.OnMoveBegin += OnCameraMoveBegin;
        _mapViewCamera.OnMoveEnd += OnCameraMoveEnd;

        //PlayerView.Freeze();
        //PlayerView.gameObject.SetActive(false);

        EnableInput();
    }

    public override void ExitState() {
        // set up camera transition response
        _mapViewCamera.OnMoveBegin -= OnCameraMoveBegin;
        _mapViewCamera.OnMoveEnd -= OnCameraMoveEnd;

        DisableInput();

        NextState = GameStates.ObjectPlace;

        base.ExitState();
    }

    public override void Dispose() {
        base.Dispose();

        PlayerView = null;
        _mapViewCamera = null;
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
        _mapView.UpdateRoomBounds(GameManager.Instance.CurrentCoord);

        EnableInput();
    }

    // TODO: make this smarter
    private void OnAxialInput(float h, float v) {
        if(_mapViewCamera.Moving) return;

        Rect roomBounds = _mapView.RoomBounds;

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
            _mapViewCamera.Move(delta);
    }

    private void OnSpecialPress() {
        NextState = GameStates.MapWalk;

        // TODO: allow player to place themselves in a new state eventually, for now just center on current screen
        //Vector3 playerPos = PlayerView.transform.position;
        //Vector2 roomCenter = _mapView.RoomBounds.center;
        //PlayerView.transform.position = new Vector3(roomCenter.x, playerPos.y, roomCenter.y);

        ExitState();
    }
}
