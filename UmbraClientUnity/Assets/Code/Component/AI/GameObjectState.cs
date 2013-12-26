using UnityEngine;
using System.Collections;
using System;

public class GameObjectState : FSMState {
    protected GameObject _gameObject;

    public GameObjectState(GameObject gameObject, Enum stateId) 
        : base(stateId) {

        _gameObject = gameObject;
    }
}
