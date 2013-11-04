using UnityEngine;
using System.Collections;

public class MapPath {
    public MapPath() { }

    public MapPath(Hashtable json) {
        FromJson(json);
    }

    public void FromJson(Hashtable json) {

    }

    public Hashtable ToJson() {
        Hashtable json = new Hashtable();

        return json;
    }
}
