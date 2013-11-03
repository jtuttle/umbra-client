using UnityEngine;
using System.Collections;

public class MapWalkState : BaseGameState {
    public PlayerView PlayerView { get; private set; }

    private MapView _mapView;
    private MapViewCamera _mapViewCamera;

    // TODO: this will probably be a minimap at some point
    private DungeonVisualizer _visualizer;

    private GameObject _otherDude;

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

        PlayerView.gameObject.SetActive(true);

        // make sure room bounds are set
        _mapView.UpdateRoomBounds(GameManager.Instance.CurrentCoord);

        // set up camera transition response
        _mapViewCamera.OnMoveBegin += OnCameraMoveBegin;
        _mapViewCamera.OnMoveEnd += OnCameraMoveEnd;

        // set up player move response
        PlayerView.OnPlayerMove += OnPlayerMove;

        // hack for other dude
        _otherDude = UnityUtils.LoadResource<GameObject>("Prefabs/OtherDude", true);
        _otherDude.transform.position = PlayerView.transform.position + new Vector3(40, 0, 0);
        GameManager.Instance.Client.PositionUpdate += UpdateOtherDudePosition;

        AddPlayerInput();
    }

    private void UpdateOtherDudePosition(string cid, string room, float px, float py, float pz, float vx, float vy, float vz) {
        _otherDude.transform.position = new Vector3(px, py, pz);
    }

    public override void ExitState() {
        _mapViewCamera.OnMoveBegin -= OnCameraMoveBegin;
        _mapViewCamera.OnMoveEnd -= OnCameraMoveEnd;

        PlayerView.OnPlayerMove -= OnPlayerMove;

        RemovePlayerInput();

        // hack for other dude
        GameManager.Instance.Client.PositionUpdate -= UpdateOtherDudePosition;

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

        GameManager.Instance.Client.SendSetPosition("noroom", position.x, position.y, position.z, velocity.x, velocity.y, velocity.z);
    }

    private void OnSpecialPress() {
        NextState = GameStates.MapDesign;
        ExitState();
    }
}
