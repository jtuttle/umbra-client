using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DungeonRoom {
    public DungeonRoom() { }

    public DungeonRoom(Hashtable json) {
        FromJson(json);
    }

    public void FromJson(Hashtable json) {

    }

    public Hashtable ToJson() {
        Hashtable json = new Hashtable();

        return json;
    }
}
