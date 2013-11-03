using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using DungeonNode = GridNode<DungeonRoom, DungeonPath>;

public class DungeonGenerator {
    public Dungeon Dungeon { get; private set; }

    private List<DungeonNode> _openNodes;

    public DungeonGenerator() {
        _openNodes = new List<DungeonNode>();
    }

    public Dungeon Generate(int numRooms) {
        Dungeon = new Dungeon();
        _openNodes.Clear();

        CreateEntrance();

        CreateRoomTree(numRooms);

        return Dungeon;
    }

    private void CreateEntrance() {
        DungeonNode entrance = Dungeon.Graph.AddNode(new XY(0, 0), new DungeonRoom(null));
        Dungeon.Entrance = entrance;
        
        _openNodes.Add(entrance);
    }

    private void CreateRoomTree(int numRooms) {
        while(Dungeon.Graph.NodeCount < numRooms)
            AddRoom();
    }

    private void AddRoom() {
        GridGraph<DungeonRoom, DungeonPath> Rooms = Dungeon.Graph;
        
        // choose random node with open edges
        DungeonNode openNode = _openNodes[Random.Range(0, _openNodes.Count)];

        // choose random open edge direction
        List<GridDirection> emptyNeighbors = GetEmptyNeighbors(openNode.Neighbors);
        GridDirection newRoomDirection = emptyNeighbors[Random.Range(0, emptyNeighbors.Count)];
        
        // add new room
        XY nextCoord = Rooms.GetCoordForNeighbor(openNode, newRoomDirection);
        
        // add new node and edges to graph
        DungeonNode newNode = Rooms.AddNode(nextCoord, new DungeonRoom());
        Rooms.AddEdge(openNode, newNode, new DungeonPath());
        Rooms.AddEdge(newNode, openNode, new DungeonPath());

        // update open nodes
        foreach(KeyValuePair<GridDirection, DungeonNode> entry in newNode.Neighbors) {
            DungeonNode neighbor = entry.Value;

            if(neighbor.Neighbors.Count == 4 && _openNodes.Contains(neighbor))
                _openNodes.Remove(neighbor);
        }

        if(Dungeon.Graph.GetNeighbors(newNode).Count != 4) _openNodes.Add(newNode);
    }

    private List<GridDirection> GetEmptyNeighbors(Dictionary<GridDirection, DungeonNode> neighbors) {
        List<GridDirection> empty = new List<GridDirection>();

        foreach(GridDirection direction in GridDirection.All) {
            if(!neighbors.ContainsKey(direction)) empty.Add(direction);
        }

        return empty;
    }
}
