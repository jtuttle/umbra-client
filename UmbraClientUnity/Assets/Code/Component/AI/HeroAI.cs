using UnityEngine;
using System.Collections;

public enum HeroState {
    Walk, Wait
}

public enum HeroTransition {
    WalkToWait, WaitToWalk
}

public class HeroAI : MonoBehaviour {
    private FiniteStateMachine<HeroState, HeroTransition> _fsm;

    protected void Awake() {
        _fsm = new FiniteStateMachine<HeroState, HeroTransition>();
    }
}
