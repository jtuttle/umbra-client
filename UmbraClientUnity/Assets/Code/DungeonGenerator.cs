using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using DungeonVertex = GridVertex<DungeonRoom, DungeonPath>;

public class DungeonGenerator {
    public Dungeon Dungeon { get; private set; }

    private List<DungeonVertex> _openVertices;

    public DungeonGenerator() {
        _openVertices = new List<DungeonVertex>();
    }

    public Dungeon Generate(int numRooms) {
        Dungeon = new Dungeon();
        _openVertices.Clear();

        CreateEntrance();

        CreateRoomTree(numRooms);

        return Dungeon;
    }

    private void CreateEntrance() {
        DungeonVertex entrance = Dungeon.Graph.AddVertex(new XY(0, 0), new DungeonRoom(null));
        Dungeon.Entrance = entrance;
        
        _openVertices.Add(entrance);
    }

    private void CreateRoomTree(int numRooms) {
        while(Dungeon.Graph.VertexCount < numRooms)
            AddRoom();
    }

    private void AddRoom() {
        GridGraph<DungeonRoom, DungeonPath> Rooms = Dungeon.Graph;

        // choose random vertex with open edges
        DungeonVertex openVertex = _openVertices[Random.Range(0, _openVertices.Count)];

        // choose random open edge direction
        List<GridDirection> emptyNeighbors = GetEmptyNeighbors(openVertex.Neighbors);
        GridDirection newRoomDirection = emptyNeighbors[Random.Range(0, emptyNeighbors.Count)];

        // add new room
        XY nextCoord = Rooms.GetCoordForNeighbor(openVertex, newRoomDirection);
        
        // add new vertex and edges to graph
        DungeonVertex newVertex = Rooms.AddVertex(nextCoord, new DungeonRoom(openVertex.Value));
        Rooms.AddEdge(openVertex, newVertex, new DungeonPath());
        Rooms.AddEdge(newVertex, openVertex, new DungeonPath());

        // update open vertices
        foreach(KeyValuePair<GridDirection, DungeonVertex> entry in newVertex.Neighbors) {
            DungeonVertex neighbor = entry.Value;

            if(neighbor.Neighbors.Count == 4 && _openVertices.Contains(neighbor))
                _openVertices.Remove(neighbor);
        }

        if(Dungeon.Graph.GetNeighbors(newVertex).Count != 4) _openVertices.Add(newVertex);
    }

    private List<GridDirection> GetEmptyNeighbors(Dictionary<GridDirection, DungeonVertex> neighbors) {
        List<GridDirection> empty = new List<GridDirection>();

        foreach(GridDirection direction in GridDirection.All) {
            if(!neighbors.ContainsKey(direction)) empty.Add(direction);
        }

        return empty;
    }
}
