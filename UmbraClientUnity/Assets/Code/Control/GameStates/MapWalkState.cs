﻿using UnityEngine;
using System.Collections;

public class MapWalkState : FSMState {
    private GameObject _player;
    private MapEntity _mapEntity;
    private TweenMover _camMover;

    // TODO: this will probably be a minimap at some point
    private MapVisualizer _visualizer;

    private Rect _roomBounds;

    public MapWalkState()
        : base(GameState.MapWalk) {

    }

    public override void EnterState(FSMState prevState) {
        base.EnterState(prevState);

        ///// init
        _player = GameManager.Instance.Player;
        _mapEntity = GameManager.Instance.Map.GetComponent<MapEntity>();
        _camMover = GameManager.Instance.GameCamera.GetComponent<TweenMover>();

        _visualizer = new MapVisualizer();
        _visualizer.RenderMap(GameManager.Instance.CurrentMap);
        /////

        // set up camera transition response
        _camMover.OnMoveBegin += OnCameraMoveBegin;
        _camMover.OnMoveEnd += OnCameraMoveEnd;

        // set up player move response
        _player.GetComponent<RigidBodyMover>().OnMove += OnPlayerMove;

        EnableInput();
    }

    public override void ExitState(FSMTransition nextStateTransition) {
        _camMover.OnMoveBegin -= OnCameraMoveBegin;
        _camMover.OnMoveEnd -= OnCameraMoveEnd;

        _player.GetComponent<RigidBodyMover>().OnMove -= OnPlayerMove;

        DisableInput();

        base.ExitState(nextStateTransition);
    }

    public override void Dispose() {
        base.Dispose();

        _player = null;
        _camMover = null;
    }

    private void EnableInput() {
        InputManager input = GameManager.Instance.Input;
        //input.GetButton(ButtonId.Attack).OnPress += _player.Attack;
        input.GetButton(ButtonId.Special).OnPress += OnSpecialPress;
    }

    private void DisableInput() {
        InputManager input = GameManager.Instance.Input;
        //input.GetButton(ButtonId.Attack).OnPress -= _player.Attack;
        input.GetButton(ButtonId.Special).OnPress -= OnSpecialPress;
    }

    private void OnCameraMoveBegin(Vector3 from, Vector3 to) {
        DisableInput();
        _player.GetComponent<PlayerInput>().Disable();
    }

    private void OnCameraMoveEnd(Vector3 from, Vector3 to) {
        GameManager.Instance.UpdateCurrentCoord(from, to);

        _player.GetComponent<PlayerInput>().Enable();
        EnableInput();
    }

    private void OnPlayerMove(Vector3 position, Vector3 velocity) {
        Rect roomBounds = _mapEntity.GetBoundsForCoord(GameManager.Instance.CurrentCoord);

        if(position.x < roomBounds.xMin)
            _camMover.Move(new XY((int)-roomBounds.width, 0));
        else if(position.x > roomBounds.xMax)
            _camMover.Move(new XY((int)roomBounds.width, 0));
        else if(position.z < roomBounds.yMin)
            _camMover.Move(new XY(0, -(int)roomBounds.height));
        else if(position.z > roomBounds.yMax)
            _camMover.Move(new XY(0, (int)roomBounds.height));
    }

    private void OnSpecialPress() {
        ExitState(new FSMTransition(GameState.MapDesign));
    }
}
