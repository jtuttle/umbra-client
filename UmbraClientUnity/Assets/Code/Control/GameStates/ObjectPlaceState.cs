using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPlaceState : BaseGameState {
    public Vector3 Placement { get; private set; }

    private List<GameObject> _options;

    private GameObject _currentOption;
    private MapView _mapView;

    public ObjectPlaceState(List<GameObject> options) 
        : base(GameStates.ObjectPlace) {

        _options = options;

        _mapView = GameObject.Find("MapView").GetComponent<MapView>();
    }

    public override void EnterState() {
        base.EnterState();

        SetOption(0);

        EnableInput();
    }

    public override void ExitState() {
        Placement = _currentOption.transform.position;

        GameObject.DestroyImmediate(_currentOption);

        DisableInput();

        base.ExitState();
    }

    public override void Dispose() {
        base.Dispose();

    }

    public void EnableInput() {
        InputManager input = GameManager.Instance.Input;
        input.OnAxialInput += OnAxialInput;
        input.GetButton(ButtonId.Confirm).OnPress += OnConfirmPress;
        input.GetButton(ButtonId.Previous).OnPress += OnPreviousPress;
        input.GetButton(ButtonId.Next).OnPress += OnNextPress;
    }

    public void DisableInput() {
        InputManager input = GameManager.Instance.Input;
        input.OnAxialInput -= OnAxialInput;
        input.GetButton(ButtonId.Confirm).OnPress -= OnConfirmPress;
        input.GetButton(ButtonId.Previous).OnPress -= OnPreviousPress;
        input.GetButton(ButtonId.Next).OnPress -= OnNextPress;
    }

    private void SetOption(int index) {
        _currentOption = _options[index];
        _currentOption = (GameObject)GameObject.Instantiate(_currentOption);

        if(_currentOption.rigidbody) {
            _currentOption.rigidbody.detectCollisions = false;
            _currentOption.rigidbody.useGravity = false;
        }

        Vector2 center = _mapView.RoomBounds.center;
        _currentOption.transform.position = new Vector3(center.x, GameConfig.BLOCK_SIZE, center.y);

        Color color = _currentOption.renderer.material.color;
        _currentOption.renderer.material.SetColor("_Color", new Color(color.r, color.g, color.b, 0.5f));
    }

    private void OnAxialInput(float h, float v) {
        _currentOption.transform.position += new Vector3(h, 0, v);
    }

    private void OnConfirmPress() {
        ExitState();
    }

    private void OnPreviousPress() {
        // previous option
    }

    private void OnNextPress() {
        // next option
    }
}
