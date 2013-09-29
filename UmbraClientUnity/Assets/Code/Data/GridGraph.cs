using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum GridDirection { N, E, S, W }
public enum SearchColor { White, Gray, Black }

public class GridVertex<T, U> {
    public XY Coord { get; private set; }
    public T Value { get; private set; }

    public Dictionary<GridDirection, GridVertex<T, U>> Neighbors { get; private set; }
    public Dictionary<GridDirection, GridEdge<T, U>> Edges { get; private set; }
    
    public GridVertex(XY coord, T value) {
        Coord = coord;
        Value = value;

        Neighbors = new Dictionary<GridDirection, GridVertex<T, U>>();
        Edges = new Dictionary<GridDirection,GridEdge<T, U>>();
    }

    public List<GridDirection> OpenEdges {
        get {
            List<GridDirection> open = new List<GridDirection>() {
                GridDirection.N, GridDirection.E, GridDirection.S, GridDirection.W
            };

            foreach(GridDirection direction in Edges.Keys)
                open.Remove(direction);

            return open;
        }
    }

    /*
    public List<GridVertex<T, U>> Neighbors {
        get {
            List<GridVertex<T, U>> neighbors = new List<GridVertex<T, U>>();

            foreach(GridEdge<T, U> edge in Edges.Values)
                neighbors.Add(edge.To);

            return neighbors;
        }
    }
    */

    public override string ToString() {
        return "Vertex @ " + Coord;
    }
}

public class GridEdge<T, U> {
    public GridVertex<T, U> From { get; private set; }
    public GridVertex<T, U> To { get; private set; }
    public U Value { get; private set; }

    public GridEdge(GridVertex<T, U> from, GridVertex<T, U> to, U value) {
        From = from;
        To = to;
        Value = value;
    }

    public GridDirection Direction {
        get {
            XY fromCoord = From.Coord;
            XY toCoord = To.Coord;

            if(fromCoord.Y < toCoord.Y)
                return GridDirection.N;
            if(fromCoord.X < toCoord.X)
                return GridDirection.E;
            if(fromCoord.Y > toCoord.Y)
                return GridDirection.S;
            if(fromCoord.X > toCoord.X)
                return GridDirection.W;

            throw new Exception("Unable to determine direction between " + fromCoord + " and " + toCoord);
        }
    }

    public override string ToString() {
        return "Edge from " + From.ToString() + " to " + To.ToString();
    }
}

public class GridGraph<T, U> {
    public Dictionary<XY, GridVertex<T, U>> Vertices { get; private set; }

    // list of vertices in graph that have at least one unused grid edge
    public List<GridVertex<T, U>> OpenVertices { get; private set; }

    public int VertexCount { get { return Vertices.Count; } }

    private int _edgeCount;
    public int EdgeCount { get { return _edgeCount; } }

    public GridGraph() {
        Vertices = new Dictionary<XY, GridVertex<T, U>>();
        
        OpenVertices = new List<GridVertex<T, U>>();
    }

    public void AddVertex(GridVertex<T, U> vertex) {
        Vertices[vertex.Coord] = vertex;

        Dictionary<GridDirection, GridVertex<T, U>> neighbors = GetNeighbors(vertex);

        foreach(KeyValuePair<GridDirection, GridVertex<T, U>> entry in neighbors) {
            vertex.Neighbors[entry.Key] = entry.Value;

            GridVertex<T, U> neighbor = entry.Value;
            neighbor.Neighbors[ReverseGridDirection(entry.Key)] = vertex;

            if(neighbor.Neighbors.Count == 4)
                OpenVertices.Remove(neighbor);
        }

        if(vertex.Neighbors.Count != 4)
            OpenVertices.Add(vertex);

        Debug.Log("added vertex @ " + vertex.Coord);
    }

    public void AddEdge(GridVertex<T, U> from, GridVertex<T, U> to, U edgeValue) {
        GridEdge<T, U> edge = new GridEdge<T, U>(from, to, edgeValue);
        from.Edges[edge.Direction] = edge;
        _edgeCount++;
    }

    public IEnumerable<GridVertex<T, U>> BreadthFirstSearch(GridVertex<T, U> root) {
        Dictionary<GridVertex<T, U>, SearchColor> visited = new Dictionary<GridVertex<T, U>, SearchColor>();

        foreach(GridVertex<T, U> vertex in Vertices.Values)
            visited[vertex] = SearchColor.White;

        Queue<GridVertex<T, U>> queue = new Queue<GridVertex<T, U>>();

        queue.Enqueue(root);
        visited[root] = SearchColor.Gray;

        while(queue.Count > 0) {
            GridVertex<T, U> next = queue.Dequeue();

            yield return next;

            List<GridVertex<T, U>> neighbors = new List<GridVertex<T, U>>(GetNeighbors(next).Values);
            
            foreach(GridVertex<T, U> neighbor in neighbors) {
                if(visited[neighbor] == SearchColor.White) {
                    visited[neighbor] = SearchColor.Gray;
                    queue.Enqueue(neighbor);
                }
            }

            visited[next] = SearchColor.Black;
        }
    }

    public XY GetCoordForNeighbor(GridVertex<T, U> from, GridDirection direction) {
        switch(direction) {
            case GridDirection.N:
                return from.Coord + new XY(0, 1);
            case GridDirection.E:
                return from.Coord + new XY(1, 0);
            case GridDirection.S:
                return from.Coord - new XY(0, 1);
            case GridDirection.W:
                return from.Coord - new XY(1, 0);
        }

        throw new Exception("Unable to determine next coord in direction " + direction);
    }

    public Dictionary<GridDirection, GridVertex<T, U>> GetNeighbors(GridVertex<T, U> vertex) {
        Dictionary<GridDirection, GridVertex<T, U>> neighbors = new Dictionary<GridDirection, GridVertex<T, U>>();

        foreach(GridDirection direction in Enum.GetValues(typeof(GridDirection))) {
            XY coord = GetCoordForNeighbor(vertex, direction);

            if(Vertices.ContainsKey(coord))
                neighbors[direction] = Vertices[coord];
        }

        return neighbors;
    }

    private GridDirection ReverseGridDirection(GridDirection direction) {
        switch(direction) {
            case GridDirection.N:
                return GridDirection.S;
            case GridDirection.E:
                return GridDirection.W;
            case GridDirection.S:
                return GridDirection.N;
            case GridDirection.W:
                return GridDirection.E;
        }

        throw new Exception("Unable to determine next coord in direction " + direction);
    }
}
