using UnityEngine;
using System.Collections;

public enum HeroState {
    Walk, Wait, Seek
}

public class HeroAI : MonoBehaviour {
    private FiniteStateMachine _fsm;

    protected void Awake() {
        _fsm = new FiniteStateMachine();

        _fsm.AddState(new HeroSeekState());
        _fsm.AddState(new HeroWalkState());
    }

    protected void Update() {
        _fsm.CurrentState.Update();
    }
}
