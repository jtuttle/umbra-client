using UnityEngine;
using System.Collections;

public class MapWalkState : BaseGameState {
    public PlayerView PlayerView { get; private set; }

    private MapView _mapView;
    private MapViewCamera _mapViewCamera;

    private XY _roomCoord;
    private Rect _roomBounds;

    // TODO: this will probably be a minimap at some point
    private DungeonVisualizer _visualizer;

    public MapWalkState(PlayerView playerView)
        : base(GameStates.MapWalk) {

        PlayerView = playerView;

        _mapView = GameObject.Find("MapView").GetComponent<MapView>();
        _mapViewCamera = GameManager.Instance.GameCamera.GetComponent<MapViewCamera>();

        _visualizer = new DungeonVisualizer();
        _visualizer.RenderDungeon(GameManager.Instance.CurrentDungeon);
    }

    public override void EnterState() {
        base.EnterState();

        _roomCoord = new XY(0, 0);
        _roomBounds = GetRoomBounds();

        PlayerView.gameObject.SetActive(true);

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
        // probably a nicer way to go about updating coordinate
        int dx = to.x < from.x ? -1 : (to.x > from.x ? 1 : 0);
        int dy = to.z < from.z ? -1 : (to.z > from.z ? 1 : 0);
        _roomCoord = _roomCoord + new XY(dx, dy);

        _roomBounds = GetRoomBounds();
        
        PlayerView.Unfreeze();
        AddPlayerInput();
    }

    private void OnPlayerMove(Vector3 position, Vector3 velocity) {
        if(position.x < _roomBounds.xMin)
            _mapViewCamera.Move(new XY((int)-_roomBounds.width, 0));
        else if(position.x > _roomBounds.xMax)
            _mapViewCamera.Move(new XY((int)_roomBounds.width, 0));
        else if(position.z < _roomBounds.yMin)
            _mapViewCamera.Move(new XY(0, -(int)_roomBounds.height));
        else if(position.z > _roomBounds.yMax)
            _mapViewCamera.Move(new XY(0, (int)_roomBounds.height));
    }

    private void OnSpecialPress() {
        NextState = GameStates.MapDesign;
        ExitState();
    }

    // this should perhaps go in mapview
    private Rect GetRoomBounds() {
        int blockSize = GameConfig.BLOCK_SIZE;

        float roomWidth = (GameConfig.ROOM_WIDTH * blockSize);
        float roomHeight = (GameConfig.ROOM_HEIGHT * blockSize);

        float left = _roomCoord.X * roomWidth - (blockSize / 2);
        float top = _roomCoord.Y * roomHeight - (blockSize / 2);

        return new Rect(left, top, roomWidth, roomHeight);
    }
}
