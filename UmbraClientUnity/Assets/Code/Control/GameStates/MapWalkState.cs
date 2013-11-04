using UnityEngine;
using System.Collections;

public class MapWalkState : BaseGameState {
    private Vector3 _playerPosition;

    private PlayerView _playerView;
    private MapView _mapView;
    private MapViewCamera _mapViewCamera;

    // TODO: this will probably be a minimap at some point
    private MapVisualizer _visualizer;

    public MapWalkState(Vector3 playerPosition)
        : base(GameStates.MapWalk) {

        _playerPosition = playerPosition;

        _mapView = GameObject.Find("MapView").GetComponent<MapView>();
        _mapViewCamera = GameManager.Instance.GameCamera.GetComponent<MapViewCamera>();

        _visualizer = new MapVisualizer();
        _visualizer.RenderMap(GameManager.Instance.CurrentMap);
    }

    public override void EnterState() {
        base.EnterState();

        PlacePlayer();

        // make sure room bounds are set
        _mapView.UpdateRoomBounds(GameManager.Instance.CurrentCoord);

        // set up camera transition response
        _mapViewCamera.OnMoveBegin += OnCameraMoveBegin;
        _mapViewCamera.OnMoveEnd += OnCameraMoveEnd;

        // set up player move response
        _playerView.OnPlayerMove += OnPlayerMove;

        EnableInput();
    }

    public override void ExitState() {
        _mapViewCamera.OnMoveBegin -= OnCameraMoveBegin;
        _mapViewCamera.OnMoveEnd -= OnCameraMoveEnd;

        _playerView.OnPlayerMove -= OnPlayerMove;

        GameObject.DestroyImmediate(_playerView.gameObject);

        DisableInput();

        base.ExitState();
    }

    public override void Dispose() {
        base.Dispose();

        _playerView = null;
        _mapViewCamera = null;
    }

    public void UpdatePlayerPosition(Vector3 position) {
        _playerPosition = position;
    }

    private void PlacePlayer() {
        _playerView = UnityUtils.LoadResource<GameObject>("Prefabs/Player", true).GetComponent<PlayerView>();
        _playerView.transform.position = _playerPosition;
    }

    private void EnableInput() {
        InputManager input = GameManager.Instance.Input;
        input.OnAxialInput += _playerView.Move;
        //input.GetButton(ButtonId.Attack).OnPress += _playerView.Attack;
        input.GetButton(ButtonId.Special).OnPress += OnSpecialPress;
    }

    private void DisableInput() {
        InputManager input = GameManager.Instance.Input;
        input.OnAxialInput -= _playerView.Move;
        //input.GetButton(ButtonId.Attack).OnPress -= _playerView.Attack;
        input.GetButton(ButtonId.Special).OnPress -= OnSpecialPress;
    }

    private void OnCameraMoveBegin(Vector3 from, Vector3 to) {
        DisableInput();
        _playerView.Freeze();
    }

    private void OnCameraMoveEnd(Vector3 from, Vector3 to) {
        GameManager.Instance.UpdateCurrentCoord(from, to);
        _mapView.UpdateRoomBounds(GameManager.Instance.CurrentCoord);

        _playerView.Unfreeze();
        EnableInput();
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
