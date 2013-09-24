using UnityEngine;
using System.Collections;

public class MapWalkState : BaseGameState {
    private MapViewCamera _mapViewCamera;
    private PlayerView _playerView;

    public MapWalkState()
        : base(GameStates.MapWalk) {


    }

    public override void EnterState() {
        base.EnterState();

        _mapViewCamera = GameManager.Instance.GameCamera.GetComponent<MapViewCamera>();
        _playerView = GameObject.FindObjectOfType(typeof(PlayerView)) as PlayerView;

        // set up camera transition response
        _mapViewCamera.OnMoveBegin += OnCameraMoveBegin;
        _mapViewCamera.OnMoveEnd += OnCameraMoveEnd;

        // set up player move response
        _playerView.OnPlayerMove += OnPlayerMove;

        AddPlayerInput();
    }

    public override void ExitState() {
        base.ExitState();

        RemovePlayerInput();
    }

    public override void Dispose() {
        base.Dispose();

        _playerView = null;
    }

    private void AddPlayerInput() {
        InputManager input = GameManager.Instance.Input;
        input.OnAxialInput += _playerView.Move;
        input.OnAttackPress += _playerView.Attack;
    }

    private void RemovePlayerInput() {
        InputManager input = GameManager.Instance.Input;
        input.OnAxialInput -= _playerView.Move;
        input.OnAttackPress -= _playerView.Attack;
    }

    private void OnCameraMoveBegin(XY delta) {
        RemovePlayerInput();
        _playerView.Freeze();
    }

    private void OnCameraMoveEnd(XY delta) {
        _playerView.Unfreeze();
        AddPlayerInput();
    }

    private void OnPlayerMove(Vector3 position, Vector3 velocity) {
        _mapViewCamera.CoverPosition(position);
    }
}
