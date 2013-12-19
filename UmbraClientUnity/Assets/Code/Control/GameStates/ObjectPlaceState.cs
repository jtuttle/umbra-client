using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPlaceState : BaseGameState {
    private List<GameObject> _options;

    private GameObject _currentOption;
    private Color _currentOptionColor;

    private MapEntity _mapView;

    private Rect _placeBounds;

    public ObjectPlaceState(List<GameObject> options, GameStates gameState)
        : base(gameState) {
            
        _options = options;

        _mapView = GameObject.Find("MapView").GetComponent<MapEntity>();
    }

    public override void EnterState() {
        base.EnterState();

        SetCurrentOption(0);
        SetPlacementBoundary();

        EnableInput();
    }

    public override void ExitState() {
        DisableInput();

        base.ExitState();
    }

    public override void Dispose() {
        _options.Clear();
        _options = null;

        _currentOption = null;
        _mapView = null;

        base.Dispose();
    }

    public void EnableInput() {
        InputManager input = GameManager.Instance.Input;
        input.OnAxialInput += OnAxialInput;
        input.GetButton(ButtonId.Confirm).OnPress += OnConfirmPress;
        input.GetButton(ButtonId.Cancel).OnPress += OnCancelPress;
        input.GetButton(ButtonId.Previous).OnPress += OnPreviousPress;
        input.GetButton(ButtonId.Next).OnPress += OnNextPress;
    }

    public void DisableInput() {
        InputManager input = GameManager.Instance.Input;
        input.OnAxialInput -= OnAxialInput;
        input.GetButton(ButtonId.Confirm).OnPress -= OnConfirmPress;
        input.GetButton(ButtonId.Cancel).OnPress -= OnCancelPress;
        input.GetButton(ButtonId.Previous).OnPress -= OnPreviousPress;
        input.GetButton(ButtonId.Next).OnPress -= OnNextPress;
    }

    private void OnAxialInput(float h, float v) {
        Vector3 oldPos = _currentOption.transform.position;

        float newX = Mathf.Clamp(oldPos.x + h * 2, _placeBounds.xMin, _placeBounds.xMax);
        float newZ = Mathf.Clamp(oldPos.z + v * 2, _placeBounds.yMin, _placeBounds.yMax);

        _currentOption.transform.position = new Vector3(newX, oldPos.y, newZ);

        // TODO: verify that placement isn't colliding with anything else and then change
        // material color to Color.red if it is or to _currentOptionColor if not
    }

    protected virtual void OnConfirmPress() {
        _currentOption.renderer.material.color = _currentOptionColor;

        // TODO: shouldn't exit if we're placing multiple items

        ExitState();
    }

    protected virtual void OnCancelPress() {
        // TODO: shouldn't exit if we're placing multiple items

        ExitState();
    }

    private void OnPreviousPress() {
        if(_options.Count < 2) return;

        // previous option
    }

    private void OnNextPress() {
        if(_options.Count < 2) return;

        // next option
    }

    private void SetCurrentOption(int index) {
        _currentOption = _options[index];
        _currentOption.SetActive(true);

        // instantiate game object if not already instantiated
        if(!_currentOption.activeInHierarchy)
            _currentOption = (GameObject)GameObject.Instantiate(_currentOption);

        // turn off physics while placing
        if(_currentOption.rigidbody) {
            _currentOption.rigidbody.detectCollisions = false;
            _currentOption.rigidbody.useGravity = false;
        }

        // start in center of room
        Vector2 center = _mapView.RoomBounds.center;
        _currentOption.transform.position = new Vector3(center.x, GameConfig.BLOCK_SIZE, center.y);

        // store current color for later use in placement validation
        _currentOptionColor = _currentOption.renderer.material.color;

        // give object some transparency
        Color placeColor = new Color(_currentOptionColor.r, _currentOptionColor.g, _currentOptionColor.b, 0.5f);
        _currentOption.renderer.material.color = placeColor;
    }

    private void SetPlacementBoundary() {
        Rect roomBounds = _mapView.RoomBounds;
        float margin = GameConfig.BLOCK_SIZE;

        _placeBounds = new Rect(roomBounds.xMin + margin * 1.5f,
                                roomBounds.yMin + margin * 1.5f,
                                roomBounds.width - margin * 3,
                                roomBounds.height - margin * 3);
    }
}
