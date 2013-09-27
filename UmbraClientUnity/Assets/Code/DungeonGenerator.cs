using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DungeonGenerator {
    public Dungeon Dungeon { get; private set; }

    public Dungeon Generate(int numRooms) {
        Dungeon = new Dungeon();

        CreateEntrance();

        CreateRoomTree(numRooms);

        return Dungeon;
    }

    private void CreateEntrance() {
        DungeonRoom entrance = new DungeonRoom(null);
        Dungeon.AddRoom(new XY(0, 0), entrance);
    }

    private void CreateRoomTree(int numRooms) {
        while(Dungeon.RoomCount < numRooms)
            AddRoom();
    }

    private void AddRoom() {
        GridGraph<DungeonRoom, DungeonPath> Rooms = Dungeon.Rooms;

        // choose random vertex with open edges
        List<GridVertex<DungeonRoom>> openVertices = Rooms.OpenVertices;
        GridVertex<DungeonRoom> openVertex = openVertices[Random.Range(0, openVertices.Count)];

        // choose random open edge direction
        List<GridDirection> openEdges = Rooms.OpenEdges(openVertex);
        GridDirection newRoomDirection = openEdges[Random.Range(0, openEdges.Count)];

        // create new graph vertex
        XY nextCoord = Rooms.GetCoordForNextVertex(openVertex, newRoomDirection);
        GridVertex<DungeonRoom> newVertex = new GridVertex<DungeonRoom>(nextCoord, new DungeonRoom(openVertex.Value));

        // add new vertex and edge to graph
        Rooms.AddVertex(newVertex);
        Rooms.AddEdge(openVertex, newVertex, new DungeonPath());

        // TODO: may not want to add edge back, but for now it's necessary to prevent the algorithm from backtracking
        Rooms.AddEdge(newVertex, openVertex, new DungeonPath());

        Debug.Log("added room @ " + nextCoord);
    }
}
