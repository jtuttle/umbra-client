using UnityEngine;
using System.Collections;

public class MapWalkState : BaseGameState {
    public PlayerView PlayerView { get; private set; }

    private MapView _mapView;
    private MapViewCamera _mapViewCamera;

    // TODO: this will probably be a minimap at some point
    private MapVisualizer _visualizer;

    public MapWalkState(PlayerView playerView)
        : base(GameStates.MapWalk) {

        PlayerView = playerView;

        _mapView = GameObject.Find("MapView").GetComponent<MapView>();
        _mapViewCamera = GameManager.Instance.GameCamera.GetComponent<MapViewCamera>();

        _visualizer = new MapVisualizer();
        _visualizer.RenderMap(GameManager.Instance.CurrentMap);
    }

    public override void EnterState() {
        base.EnterState();

        PlayerView.gameObject.SetActive(true);

        // make sure room bounds are set
        _mapView.UpdateRoomBounds(GameManager.Instance.CurrentCoord);

        // set up camera transition response
        _mapViewCamera.OnMoveBegin += OnCameraMoveBegin;
        _mapViewCamera.OnMoveEnd += OnCameraMoveEnd;

        // set up player move response
        PlayerView.OnPlayerMove += OnPlayerMove;

        AddPlayerInput();
    }

    public override void ExitState() {
        _mapViewCamera.OnMoveBegin -= OnCameraMoveBegin;
        _mapViewCamera.OnMoveEnd -= OnCameraMoveEnd;

        PlayerView.OnPlayerMove -= OnPlayerMove;

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
        input.OnAxialInput += PlayerView.Move;
        input.OnAttackPress += PlayerView.Attack;
        input.OnSpecialPress += OnSpecialPress;
    }

    private void RemovePlayerInput() {
        InputManager input = GameManager.Instance.Input;
        input.OnAxialInput -= PlayerView.Move;
        input.OnAttackPress -= PlayerView.Attack;
        input.OnSpecialPress -= OnSpecialPress;
    }

    private void OnCameraMoveBegin(Vector3 from, Vector3 to) {
        RemovePlayerInput();
        PlayerView.Freeze();
    }

    private void OnCameraMoveEnd(Vector3 from, Vector3 to) {
        GameManager.Instance.UpdateCurrentCoord(from, to);
        _mapView.UpdateRoomBounds(GameManager.Instance.CurrentCoord);

        PlayerView.Unfreeze();
        AddPlayerInput();
    }

    private void OnPlayerMove(Vector3 position, Vector3 velocity) {
        Rect roomBounds = _mapView.RoomBounds;

        if(position.x < roomBounds.xMin)
            _mapViewCamera.Move(new XY((int)-roomBounds.width, 0));
        else if(position.x > roomBounds.xMax)
            _mapViewCamera.Move(new XY((int)roomBounds.width, 0));
        else if(position.z < roomBounds.yMin)
            _mapViewCamera.Move(new XY(0, -(int)roomBounds.height));
        else if(position.z > roomBounds.yMax)
            _mapViewCamera.Move(new XY(0, (int)roomBounds.height));
    }

    private void OnSpecialPress() {
        NextState = GameStates.MapDesign;
        ExitState();
    }
}
