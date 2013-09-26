using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum DungeonDirections { N, E, S, W }

public class Dungeon {
    public Dictionary<XY, DungeonRoom> Rooms { get; private set; }

    public int RoomCount { get { return Rooms.Keys.Count; } }

    public Dungeon() {
        Rooms = new Dictionary<XY, DungeonRoom>();
    }

    public void AddRoom(DungeonRoom room) {
        Rooms[room.Coords] = room;
    }
}
