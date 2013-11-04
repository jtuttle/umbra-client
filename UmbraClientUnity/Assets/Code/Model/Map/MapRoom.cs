using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MapRoom {
    public MapRoom() { }

    public MapRoom(Hashtable json) {
        FromJson(json);
    }

    public void FromJson(Hashtable json) {

    }

    public Hashtable ToJson() {
        Hashtable json = new Hashtable();

        return json;
    }
}
