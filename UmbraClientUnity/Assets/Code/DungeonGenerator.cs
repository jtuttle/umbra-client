using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DungeonGenerator {
    public Dungeon Dungeon { get; private set; }

    private List<DungeonRoom> _openRooms;

    public Dungeon Generate(int numRooms) {
        Dungeon = new Dungeon();
        _openRooms = new List<DungeonRoom>();

        CreateEntrance();

        CreateRoomTree(numRooms);

        return Dungeon;
    }

    private void CreateEntrance() {
        DungeonRoom entrance = new DungeonRoom(new XY(0, 0), null, null);
        Dungeon.AddRoom(entrance);
        _openRooms.Add(entrance);
    }

    private void CreateRoomTree(int numRooms) {
        while(Dungeon.RoomCount < numRooms)
            AddRoom();
    }

    private void AddRoom() {
        DungeonRoom openRoom = _openRooms[Random.Range(0, _openRooms.Count)];

        List<DungeonDirections> openEdges = openRoom.OpenEdges;
        DungeonDirections openEdgeDirection = openEdges[Random.Range(0, openEdges.Count)];

        DungeonEdge edge = new DungeonEdge();
        DungeonRoom newRoom = new DungeonRoom(openRoom.GetCoordForDirection(openEdgeDirection), openRoom, edge);

        openRoom.AddChild(newRoom, edge, openEdgeDirection);
        Dungeon.AddRoom(newRoom);

        // remove old open room if we juse used its last edge
        if(openEdges.Count == 1)
            _openRooms.Remove(openRoom);

        // add new open room if it has empty edges
        if(newRoom.OpenEdges.Count > 0)
            _openRooms.Add(newRoom);

        Debug.Log("added room @ " + newRoom.Coords);
    }
}
