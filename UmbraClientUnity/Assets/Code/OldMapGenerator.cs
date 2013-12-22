using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using MapNode = GridNode<MapRoom, MapPath>;

public class OldMapGenerator {
    public Map Map { get; private set; }

    private List<MapNode> _openNodes;

    public OldMapGenerator() {
        _openNodes = new List<MapNode>();
    }

    public Map Generate(int numRooms) {
        Map = new Map();
        _openNodes.Clear();

        // IDEA: place goal and boss rooms FIRST, then create rest of dungeon and place entrance randomly

        CreateEntrance();

        CreateRoomTree(numRooms);

        CreateGoal();

        return Map;
    }

    private void CreateEntrance() {
        MapNode entrance = Map.Graph.AddNode(new XY(0, 0), new MapRoom(MapRoomSymbol.Entrance));
        Map.Entrance = entrance;
        
        _openNodes.Add(entrance);
    }

    private void CreateGoal() {
        List<MapNode> goalCandidates = GetGoalCandidates();

        if(goalCandidates.Count == 0) {
            Debug.Log("FAIL");
            return;
        }

        MapNode goal = goalCandidates[Random.Range(0, goalCandidates.Count)];
        MapNode bossRoom = goal.GetNeighborList()[0];

        goal.Data.Symbol = MapRoomSymbol.Goal;
        bossRoom.Data.Symbol = MapRoomSymbol.Boss;
    }

    private void CreateRoomTree(int numRooms) {
        while(Map.Graph.NodeCount < numRooms)
            AddRoom();
    }

    private void AddRoom() {
        GridGraph<MapRoom, MapPath> Rooms = Map.Graph;
        
        // choose random node with open edges
        MapNode openNode = _openNodes[Random.Range(0, _openNodes.Count)];

        // choose random open edge direction
        List<GridDirection> emptyNeighbors = GetEmptyNeighbors(openNode.Neighbors);
        GridDirection newRoomDirection = emptyNeighbors[Random.Range(0, emptyNeighbors.Count)];
        
        // add new room
        XY nextCoord = Rooms.GetCoordForNeighbor(openNode, newRoomDirection);
        
        // add new node and edges to graph
        MapNode newNode = Rooms.AddNode(nextCoord, new MapRoom());
        Rooms.AddEdge(openNode, newNode, new MapPath());
        Rooms.AddEdge(newNode, openNode, new MapPath());

        // update open nodes
        foreach(KeyValuePair<GridDirection, MapNode> entry in newNode.Neighbors) {
            MapNode neighbor = entry.Value;

            if(neighbor.Neighbors.Count == 4 && _openNodes.Contains(neighbor))
                _openNodes.Remove(neighbor);
        }

        if(Map.Graph.GetNeighbors(newNode).Count != 4) _openNodes.Add(newNode);
    }

    private List<GridDirection> GetEmptyNeighbors(Dictionary<GridDirection, MapNode> neighbors) {
        List<GridDirection> empty = new List<GridDirection>();

        foreach(GridDirection direction in GridDirection.All) {
            if(!neighbors.ContainsKey(direction)) empty.Add(direction);
        }

        return empty;
    }

    // TODO: speed this up by keeping track of leaf nodes
    private List<MapNode> GetGoalCandidates() {
        List<MapNode> goals = new List<MapNode>();

        foreach(MapNode node in Map.Graph.BreadthFirstSearch(Map.Entrance)) {
            // leaf nodes with 1 neighbor whose singular neighbor only has 1 other neighbor
            if(node.Data.Symbol == MapRoomSymbol.None && node.EdgeCount == 1) {
                MapNode neighbor = node.GetEdgeList()[0].To;

                if(neighbor.Data.Symbol == MapRoomSymbol.None && neighbor.EdgeCount == 2)
                    goals.Add(node);
            }
        }

        return goals;
    }
}
