using UnityEngine;
using System.Collections;

public class MapWalkState : BaseGameState {
    public PlayerView PlayerView { get; private set; }
    private MapViewCamera _mapViewCamera;

    // TODO: this will probably be a minimap at some point
    private DungeonVisualizer _visualizer;

    public MapWalkState(PlayerView playerView)
        : base(GameStates.MapWalk) {

        PlayerView = playerView;

        _mapViewCamera = GameManager.Instance.GameCamera.GetComponent<MapViewCamera>();

        _visualizer = new DungeonVisualizer();
        _visualizer.RenderDungeon(GameManager.Instance.CurrentDungeon);
    }

    public override void EnterState() {
        base.EnterState();

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
        input.OnMapViewPress += OnMapViewPress;
    }

    private void RemovePlayerInput() {
        InputManager input = GameManager.Instance.Input;
        input.OnAxialInput -= PlayerView.Move;
        input.OnAttackPress -= PlayerView.Attack;
        input.OnSpecialPress -= OnSpecialPress;
        input.OnMapViewPress -= OnMapViewPress;
    }

    private void OnCameraMoveBegin(XY delta) {
        RemovePlayerInput();
        PlayerView.Freeze();
    }

    private void OnCameraMoveEnd(XY delta) {
        PlayerView.Unfreeze();
        AddPlayerInput();
    }

    private void OnPlayerMove(Vector3 position, Vector3 velocity) {
        _mapViewCamera.CoverPosition(position);
    }

    private void OnSpecialPress() {
        NextState = GameStates.MapDesign;
        ExitState();
    }

    private void OnMapViewPress() {
        NextState = GameStates.MapView;
        ExitState();
    }
}
