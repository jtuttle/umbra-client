using System;
using UnityEngine;

public class HeroWalkState : GameObjectState {
    private RigidBodyMover _mover;

    private Vector3 _destination;

    public HeroWalkState(GameObject hero) : 
        base(hero, HeroState.Walk) {

        _mover = _gameObject.GetComponent<RigidBodyMover>();
    }

    public override void EnterState(FSMState prevState) {
        base.EnterState(prevState);

        if((HeroState)prevState.StateId == HeroState.Seek)
            _destination = (prevState as HeroSeekState).Destination;
    }

    public override void ExitState(FSMTransition nextStateTransition) {

        base.ExitState(nextStateTransition);
    }

    public override void Update() {
        Move();

        if(AtDestination())
            ExitState(new FSMTransition(HeroState.Seek));
    }

    public override void Dispose() {
        
    }

    private void Move() {
        float xDiff = _destination.x - _gameObject.transform.position.x;
        float zDiff = _destination.z - _gameObject.transform.position.z;
        float angle = Mathf.Atan2(zDiff, xDiff);

        float x = (float)Math.Cos(angle);
        float z = (float)Math.Sin(angle);

        _mover.Move(x, z);
    }

    private bool AtDestination() {
        float xDiff = _destination.x - _gameObject.transform.position.x;
        float zDiff = _destination.z - _gameObject.transform.position.z;

        return Math.Abs(xDiff) < 1 && Math.Abs(zDiff) < 1;
    }
}
