using UnityEngine;
using System.Collections;

public enum MonsterState {
    Wander
}

public class MonsterAI : MonoBehaviour {
    private FiniteStateMachine _fsm;

    protected void Awake() {
        _fsm = new FiniteStateMachine();

        _fsm.AddState(new MonsterWanderState(gameObject));

        _fsm.ChangeState(new FSMTransition(MonsterState.Wander));
    }

    protected void Update() {
        if(_fsm != null)
            _fsm.Update();
    }
}
