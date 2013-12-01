using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using MapNode = GridNode<MapRoom, MapPath>;
using MapEdge = GridEdge<MapRoom, MapPath>;

public class Map : IJsonable {
    public MapNode Entrance { get; set; }

    public GridGraph<MapRoom, MapPath> Graph { get; private set; }
    
    public Map() {
        Graph = new GridGraph<MapRoom, MapPath>();
    }

    public Map(Hashtable json)
        : this() {

        FromJson(json);
    }

    public void FromJson(Hashtable json) {
        ArrayList nodes = json["Nodes"] as ArrayList;

        foreach(Hashtable nodeHash in nodes) {
            XY coord = new XY(nodeHash["Coord"] as Hashtable);
            MapRoom room = new MapRoom(nodeHash["Room"] as Hashtable);

            Graph.AddNode(coord, room);
        }

        ArrayList edges = json["Edges"] as ArrayList;

        foreach(Hashtable edgeHash in edges) {
            MapNode from = Graph.GetNodeByCoord(new XY(edgeHash["From"] as Hashtable));
            MapNode to = Graph.GetNodeByCoord(new XY(edgeHash["To"] as Hashtable));
            MapPath path = new MapPath(edgeHash["Path"] as Hashtable);

            Graph.AddEdge(from, to, path);
        }

        Entrance = Graph.GetNodeByCoord(new XY(json["Entrance"] as Hashtable));
    }

    public Hashtable ToJson() {
        Hashtable json = new Hashtable();

        ArrayList nodes = new ArrayList();
        ArrayList edges = new ArrayList();

        foreach(MapNode node in Graph.BreadthFirstSearch(Entrance)) {
            Hashtable nodeHash = new Hashtable();

            nodeHash["Coord"] = node.Coord.ToJson();
            nodeHash["Room"] = node.Data.ToJson();

            nodes.Add(nodeHash);

            foreach(KeyValuePair<GridDirection, MapEdge> entry in node.Edges) {
                Hashtable edgeHash = new Hashtable();

                MapEdge edge = entry.Value;

                XY from = edge.From.Coord;
                XY to = edge.To.Coord;
                MapPath path = edge.Data;

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
