using UnityEngine;
using System.Collections;

public enum HeroState {
    Walk, Wait, Seek
}

public class HeroAI : MonoBehaviour {
    private FiniteStateMachine _fsm;

    protected void Awake() {
        _fsm = new FiniteStateMachine();

        _fsm.AddState(new HeroSeekState(gameObject));
        _fsm.AddState(new HeroWalkState(gameObject));
        _fsm.AddState(new HeroWaitState(gameObject));

        _fsm.ChangeState(HeroState.Seek);
    }

    protected void Update() {
        _fsm.Update();
    }
}
