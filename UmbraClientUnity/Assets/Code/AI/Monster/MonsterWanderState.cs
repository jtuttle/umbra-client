using System;
using UnityEngine;

public class MonsterWanderState : GameObjectState {
    private RigidBodyMover _mover;

    private Vector3 _destination;

    public MonsterWanderState(GameObject monster) :
        base(monster, MonsterState.Wander) {

        _mover = _gameObject.GetComponent<RigidBodyMover>();
    }

    public override void EnterState(FSMState prevState) {
        base.EnterState(prevState);

        SetNewDestination();
    }

    public override void ExitState(FSMTransition nextStateTransition) {

        base.ExitState(nextStateTransition);
    }

    public override void Update() {
        Move();

        if(AtDestination())
            SetNewDestination();
    }

    public override void Dispose() {

    }

    private void SetNewDestination() {
        MapEntity mapEntity = GameManager.Instance.Map.GetComponent<MapEntity>();

        XY coord = mapEntity.GetCoordFromPosition(_gameObject.transform.position);
        Rect bounds = mapEntity.GetBoundsForCoord(coord, 1);

        float xRand = UnityEngine.Random.Range(bounds.xMin, bounds.xMax);
        float zRand = UnityEngine.Random.Range(bounds.yMin, bounds.yMax);

        _destination = new Vector3(xRand, 0, zRand);
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
