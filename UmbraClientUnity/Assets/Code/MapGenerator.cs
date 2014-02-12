using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using MapNode = GridNode<MapRoom, MapPath>;

public class MapGenerator {
    public Map Map { get; private set; }

    public MapGenerator() {

    }

    public Map Generate(int width, int height) {
        Map = new Map();
        
        // initial setup
        XY entrance = new XY(0, 0);

        // create critical path
        int dimensionMax = Mathf.Max(width, height);
        int pathLength = dimensionMax + (dimensionMax / 2);
        List<XY> critPath = GetPathWithLength(entrance, pathLength);

        // build graph structure
        for(int i = 0; i < critPath.Count; i++) {
            MapRoomSymbol symbol = MapRoomSymbol.None;

            if(i == 0)
                symbol = MapRoomSymbol.Entrance;
            else if(i == critPath.Count - 2)
                symbol = MapRoomSymbol.Boss;
            else if(i == critPath.Count - 1)
                symbol = MapRoomSymbol.Goal;

            XY current = critPath[i];
            MapNode currentNode = Map.Graph.AddNode(current, new MapRoom(symbol));

            if(i > 0) {
                XY prev = critPath[i - 1];
                MapNode prevNode = Map.Graph.GetNodeByCoord(prev);

                Map.Graph.AddEdge(currentNode, prevNode, new MapPath());
                Map.Graph.AddEdge(prevNode, currentNode, new MapPath());
            } else {
                Map.Entrance = currentNode;
            }
        }

        // TODO
        // mark remaining goal and boss room edges as invalid
        // add key levels, keys
        // create deviations from critical path
        // create shortcuts, secret rooms, etc

        return Map;
    }

    private List<XY> GetPathWithLength(XY start, int length) {
        Stack<XY> path = new Stack<XY>();
        path.Push(start);

        Dictionary<XY, bool> visited = new Dictionary<XY, bool>();

        while(path.Count < length) {
            XY current = path.Peek();
            visited[current] = true;

            List<XY> neighbors = current.Neighbors;
            neighbors = neighbors.Where(n => !visited.ContainsKey(n)).ToList();

            if(neighbors.Count == 0) {
                path.Pop();
            } else {
                XY next = neighbors[Random.Range(0, neighbors.Count)];
                path.Push(next);
            }
        }
        
        return new List<XY>(path.ToArray().Reverse());
    }

    private List<XY> GetTilesWithManhattanDistance(XY start, int distance) {
        List<XY> tiles = new List<XY>();

        Queue<XY> tilesToCheck = new Queue<XY>();
        tilesToCheck.Enqueue(start);

        List<XY> tilesChecked = new List<XY>();

        while(tilesToCheck.Count > 0) {
            XY next = tilesToCheck.Dequeue();

            if(tilesChecked.Contains(next)) continue;

            tilesChecked.Add(next);

            int manhattanDistance = Mathf.Abs(next.X - start.X) + Mathf.Abs(next.Y - start.Y);
            
            if(manhattanDistance == distance) {
                tiles.Add(next);
            } else {
                List<XY> neighbors = next.Neighbors;

                foreach(XY neighbor in neighbors) {
                    if(!tilesChecked.Contains(neighbor))
                        tilesToCheck.Enqueue(neighbor);
                }
            }
        }

        return tiles;
    }
}
