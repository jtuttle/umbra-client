using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using DungeonVertex = GridVertex<DungeonRoom, DungeonPath>;

public class DungeonGenerator {
    public Dungeon Dungeon { get; private set; }

    public Dungeon Generate(int numRooms) {
        Dungeon = new Dungeon();

        CreateEntrance();

        CreateRoomTree(numRooms);

        return Dungeon;
    }

    private void CreateEntrance() {
        DungeonVertex entrance = new DungeonVertex(new XY(0, 0), new DungeonRoom(null));
        Dungeon.Entrance = entrance;
        Dungeon.Graph.AddVertex(entrance);
    }

    private void CreateRoomTree(int numRooms) {
        while(Dungeon.Graph.VertexCount < numRooms)
            AddRoom();
    }

    private void AddRoom() {
        GridGraph<DungeonRoom, DungeonPath> Rooms = Dungeon.Graph;

        // choose random vertex with open edges
        List<DungeonVertex> openVertices = Rooms.OpenVertices;
        DungeonVertex openVertex = openVertices[Random.Range(0, openVertices.Count)];

        // choose random open edge direction
        List<GridDirection> openEdges = openVertex.OpenEdges;
        GridDirection newRoomDirection = openEdges[Random.Range(0, openEdges.Count)];

        // add new room
        XY nextCoord = Rooms.GetCoordForNeighbor(openVertex, newRoomDirection);
        DungeonVertex newVertex = new DungeonVertex(nextCoord, new DungeonRoom(openVertex.Value));
        
        // add new vertex and edges to graph
        Rooms.AddVertex(newVertex);
        Rooms.AddEdge(openVertex, newVertex, new DungeonPath());
        Rooms.AddEdge(newVertex, openVertex, new DungeonPath());
    }
}
