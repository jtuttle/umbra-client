using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using DungeonVertex = GridVertex<DungeonRoom, DungeonPath>;

public class Dungeon {
    public DungeonVertex Entrance { get; set; }

    public GridGraph<DungeonRoom, DungeonPath> Graph { get; private set; }
    
    public Dungeon() {
        Graph = new GridGraph<DungeonRoom, DungeonPath>();
    }
}
