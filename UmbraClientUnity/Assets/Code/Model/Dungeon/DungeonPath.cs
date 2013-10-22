using UnityEngine;
using System.Collections;

public class DungeonPath {
    public DungeonPath() { }

    public DungeonPath(Hashtable json) {
        FromJson(json);
    }

    public void FromJson(Hashtable json) {

    }

    public Hashtable ToJson() {
        Hashtable json = new Hashtable();

        return json;
    }
}
