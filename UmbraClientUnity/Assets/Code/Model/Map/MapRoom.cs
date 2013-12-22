using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum MapRoomSymbol {
    None, Entrance, Boss, Goal
}

public class MapRoom {
    public MapRoomSymbol Symbol { get; set; }

    public MapRoom(MapRoomSymbol symbol = MapRoomSymbol.None) {
        Symbol = symbol;
    }

    public MapRoom(Hashtable json) {
        FromJson(json);
    }

    public void FromJson(Hashtable json) {
        if(json.ContainsKey("symbol"))
            Symbol = (MapRoomSymbol)Enum.Parse(typeof(MapRoomSymbol), json["symbol"].ToString());
    }

    public Hashtable ToJson() {
        Hashtable json = new Hashtable();

        if(Symbol != null) json["symbol"] = Symbol.ToString();

        return json;
    }
}
