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

    public override void ExitState(Enum nextState) {

        base.ExitState(nextState);
    }

    public override void Update() {
        Move();

        if(AtDestination())
            ExitState(HeroState.Seek);
    }

    public override void Dispose() {
        
    }

    private void Move() {
        float xDiff = _destination.x - _gameObject.transform.position.x;
        float zDiff = _destination.z - _gameObject.transform.position.z;
        float angle = Mathf.Atan2(zDiff, xDiff);

        float x = _mover.Speed * (float)Math.Cos(angle);
        float z = _mover.Speed * (float)Math.Sin(angle);

        if(Math.Abs(xDiff) < _mover.Speed) x = xDiff;
        if(Math.Abs(zDiff) < _mover.Speed) z = zDiff;

        _mover.Move(x, z);
    }

    private bool AtDestination() {
        float xDiff = _destination.x - _gameObject.transform.position.x;
        float zDiff = _destination.z - _gameObject.transform.position.z;

        return Math.Abs(xDiff) < 1 && Math.Abs(zDiff) < 1;
    }
}
