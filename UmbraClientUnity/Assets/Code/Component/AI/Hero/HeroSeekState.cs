using System;
using UnityEngine;

using MapNode = GridNode<MapRoom, MapPath>;
using MapEdge = GridEdge<MapRoom, MapPath>;
using System.Collections.Generic;

public class HeroSeekState : GameObjectState {
    public Vector3 Destination { get; private set; }

    public HeroSeekState(GameObject hero)
        : base(hero, HeroState.Seek) {

    }

    public override void EnterState(FSMState prevState) {
        Vector3 destination = FindDestination();
    }

    public override void ExitState(Enum nextState) {

        base.ExitState(nextState);
    }

    public override void Update() {
        
    }

    public override void Dispose() {
        
    }

    private Vector3 FindDestination() {
        // 1) get current room from gameobject coordinates (need to give gameobject access to state)
        MapEntity mapEntity = GameManager.Instance.Map.GetComponent<MapEntity>();
        XY currentCoord = mapEntity.GetCoordFromPosition(_gameObject.transform.position);

        // 2) choose next room to explore
        MapNode currentNode = mapEntity.MapModel.Graph.GetNodeByCoord(currentCoord);
        List<MapEdge> paths = currentNode.GetEdgeList();
        XY nextCoord = paths[UnityEngine.Random.Range(0, paths.Count)].To.Coord;

        // 3) store center of chosen room
        Vector2 nextCenter = mapEntity.GetBoundsForCoord(nextCoord).center;
        Destination = new Vector3(nextCenter.x, 0, nextCenter.y);

        // 4) exit state 
        ExitState(HeroState.Walk);

        return Vector3.zero;
    }
}
