using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum DungeonDirections { N, E, S, W }

public class Dungeon {
    public DungeonRoom Entrance { get; private set; }

    public GridGraph<DungeonRoom, DungeonPath> Rooms { get; private set; }
    public int RoomCount { get { return Rooms.VertexCount; } }
    
    public Dungeon() {
        Rooms = new GridGraph<DungeonRoom, DungeonPath>();
    }

    public void AddRoom(XY coord, DungeonRoom room) {
        if(RoomCount == 0)
            Entrance = room;

        Rooms.AddVertex(new GridVertex<DungeonRoom>(coord, room));
    }
}
