using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DungeonRoom {
    public DungeonRoom Parent { get; private set; }

    public DungeonRoom(DungeonRoom parent) {
        Parent = parent;
    }
}
