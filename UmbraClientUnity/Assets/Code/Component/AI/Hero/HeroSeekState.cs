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
        base.EnterState(prevState);

    }

    public override void ExitState(Enum nextState) {

        base.ExitState(nextState);
    }

    public override void Update() {
        Destination = FindDestination();
        ExitState(HeroState.Walk);
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

        // 3) get center of chosen room
        Vector2 nextCenter = mapEntity.GetBoundsForCoord(nextCoord).center;
        
        return new Vector3(nextCenter.x, 0, nextCenter.y);
    }
}
