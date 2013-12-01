using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using MapNode = GridNode<MapRoom, MapPath>;

public class MapGenerator {
    public Map Map { get; private set; }

    private List<MapNode> _openNodes;

    public MapGenerator() {
        _openNodes = new List<MapNode>();
    }

    public Map Generate(int numRooms) {
        Map = new Map();
        _openNodes.Clear();

        CreateEntrance();

        CreateRoomTree(numRooms);

        return Map;
    }

    private void CreateEntrance() {
        MapNode entrance = Map.Graph.AddNode(new XY(0, 0), new MapRoom(null));
        Map.Entrance = entrance;
        
        _openNodes.Add(entrance);
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
}
