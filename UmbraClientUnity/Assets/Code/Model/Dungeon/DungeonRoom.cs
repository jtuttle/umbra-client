using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DungeonRoom {
    public XY Coords { get; private set; }

    public DungeonRoom Parent { get; private set; }
    public Dictionary<DungeonDirections, DungeonRoom> Children { get; private set; }
    public Dictionary<DungeonDirections, DungeonEdge> Edges { get; private set; }

    public DungeonRoom(XY coords, DungeonRoom parent, DungeonEdge edge) {
        Coords = coords;
        Parent = parent;

        Children = new Dictionary<DungeonDirections, DungeonRoom>();
        Edges = new Dictionary<DungeonDirections, DungeonEdge>();

        if(Parent != null) Edges[GetNeighborDirection(Parent)] = edge;
    }

    public List<DungeonDirections> OpenEdges {
        get {
            List<DungeonDirections> open = new List<DungeonDirections>() {
                DungeonDirections.N, DungeonDirections.E, DungeonDirections.S, DungeonDirections.W
            };

            foreach(DungeonDirections direction in Edges.Keys) {
                open.Remove(direction);
            }

            return open;
        }
    }

    public void AddChild(DungeonRoom room, DungeonEdge edge, DungeonDirections direction) {
        Children[direction] = room;
        Edges[direction] = edge;
    }

    private DungeonDirections GetNeighborDirection(DungeonRoom neighbor) {
        if(neighbor.Coords.Y > Coords.Y)
            return DungeonDirections.N;
        else if(neighbor.Coords.X > Coords.X)
            return DungeonDirections.E;
        else if(neighbor.Coords.Y < Coords.Y)
            return DungeonDirections.S;
        else if(neighbor.Coords.X < Coords.X)
            return DungeonDirections.W;

        throw new Exception("Rooms have same coordinates");
    }

    public XY GetCoordForDirection(DungeonDirections direction) {
        switch(direction) {
            case DungeonDirections.N:
                return Coords + new XY(0, 1);
            case DungeonDirections.E:
                return Coords + new XY(1, 0);
            case DungeonDirections.S:
                return Coords - new XY(0, 1);
            case DungeonDirections.W:
                return Coords - new XY(1, 0);
        }

        throw new Exception("Invalid direction");
    }
}
