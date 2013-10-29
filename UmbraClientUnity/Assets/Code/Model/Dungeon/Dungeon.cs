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

    public Dungeon(Hashtable json)
        : this() {

        FromJson(json);
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
            DungeonVertex from = Graph.GetVertexByCoord(new XY(edgeHash["From"] as Hashtable));
            DungeonVertex to = Graph.GetVertexByCoord(new XY(edgeHash["To"] as Hashtable));
            DungeonPath path = new DungeonPath(edgeHash["Path"] as Hashtable);

            Graph.AddEdge(from, to, path);
        }

        Entrance = Graph.GetVertexByCoord(new XY(json["Entrance"] as Hashtable));
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

            foreach(KeyValuePair<GridDirection, DungeonEdge> entry in node.Edges) {
                Hashtable edgeHash = new Hashtable();

                DungeonEdge edge = entry.Value;

                XY from = edge.From.Coord;
                XY to = edge.To.Coord;
                DungeonPath path = edge.Data;

                edgeHash["From"] = from.ToJson();
                edgeHash["To"] = to.ToJson();
                edgeHash["Path"] = path.ToJson();

                edges.Add(edgeHash);
            }
        }

        json["Nodes"] = nodes;
        json["Edges"] = edges;

        json["Entrance"] = Entrance.Coord.ToJson();

        return json;
    }
}
