using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using DungeonVertex = GridVertex<DungeonRoom, DungeonPath>;
using DungeonEdge = GridEdge<DungeonRoom, DungeonPath>;

public class Dungeon : IJsonable {
    public DungeonVertex Entrance { get; set; }

    public GridGraph<DungeonRoom, DungeonPath> Graph { get; private set; }
    
    public Dungeon() {
        Graph = new GridGraph<DungeonRoom, DungeonPath>();
    }

    public void FromJson(Hashtable json) {
        ArrayList nodes = json["Nodes"] as ArrayList;

        foreach(Hashtable nodeHash in nodes) {
            XY coord = new XY(nodeHash["Coord"] as Hashtable);
            DungeonRoom room = new DungeonRoom(nodeHash["Room"] as Hashtable);
            Graph.AddVertex(coord, room);
        }

        ArrayList edges = json["Edges"] as ArrayList;

        foreach(Hashtable edgeHash in edges) {
            DungeonPath path = new DungeonPath(edgeHash);
            //Graph.AddEdge(
        }
    }

    public Hashtable ToJson() {
        Hashtable json = new Hashtable();

        ArrayList nodes = new ArrayList();
        ArrayList edges = new ArrayList();

        foreach(DungeonVertex node in Graph.BreadthFirstSearch(Entrance)) {
            Hashtable nodeHash = new Hashtable();

            nodeHash["Coord"] = node.Coord.ToJson();
            nodeHash["Room"] = node.Data.ToJson();

            nodes.Add(nodeHash);

            Hashtable edgeHash = new Hashtable();

            foreach(KeyValuePair<GridDirection, DungeonEdge> edge in node.Edges) {
                DungeonPath path = edge.Value.Data;
                edgeHash[edge.Key] = path.ToJson();
            }
        }

        json["Nodes"] = nodes;
        json["Edges"] = edges;

        return json;
    }
}
