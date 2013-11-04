using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPlaceState : BaseGameState {
    public GameObject CurrentOption { get; private set; }
    public Vector3 Placement { get; private set; }

    private List<GameObject> _options;

    public ObjectPlaceState(Map map, List<GameObject> options) 
        : base(GameStates.ObjectPlace) {

        _options = options;
    }

    public override void EnterState() {
        base.EnterState();

        CurrentOption = _options[0];
        
        // show visual

        EnableInput();
    }

    public override void ExitState() {
        // set placement

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

    private void OnAxialInput(float h, float v) {
        // move object around
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
