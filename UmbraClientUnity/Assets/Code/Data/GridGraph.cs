using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum SearchColor { White, Gray, Black }

public sealed class GridDirection {
    public static readonly GridDirection N = new GridDirection();
    public static readonly GridDirection E = new GridDirection();
    public static readonly GridDirection S = new GridDirection();
    public static readonly GridDirection W = new GridDirection();

    private GridDirection() { }

    public override string ToString() {
        if(this == N) return "N";
        if(this == E) return "E";
        if(this == S) return "S";
        if(this == W) return "W";
        throw new Exception("Unable to stringify direction");
    }

    public GridDirection Reverse() {
        if(this == N) return S;
        if(this == E) return W;
        if(this == S) return N;
        if(this == W) return E;
        throw new Exception("Unable to reverse direction");
    }

    public static List<GridDirection> All {
        get { return new List<GridDirection>() { N, E, S, W }; }
    }
}

public class GridVertex<T, U> {
    public XY Coord { get; private set; }
    public T Value { get; private set; }

    public Dictionary<GridDirection, GridEdge<T, U>> Edges { get; private set; }
    public Dictionary<GridDirection, GridVertex<T, U>> Neighbors { get; private set; }
    
    public GridVertex(XY coord, T value) {
        Coord = coord;
        Value = value;

        Edges = new Dictionary<GridDirection,GridEdge<T, U>>();
        Neighbors = new Dictionary<GridDirection, GridVertex<T, U>>();
    }

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

            if(fromCoord.Y < toCoord.Y) return GridDirection.N;
            if(fromCoord.X < toCoord.X) return GridDirection.E;
            if(fromCoord.Y > toCoord.Y) return GridDirection.S;
            if(fromCoord.X > toCoord.X) return GridDirection.W;

            throw new Exception("Unable to determine direction between " + fromCoord + " and " + toCoord);
        }
    }

    public override string ToString() {
        return "Edge from " + From.ToString() + " to " + To.ToString();
    }
}

public class GridGraph<T, U> {
    private Dictionary<XY, GridVertex<T, U>> _coordIndex;
    private Dictionary<T, GridVertex<T, U>> _valueIndex;
    public int VertexCount { get { return _coordIndex.Count; } }

    private int _edgeCount;
    public int EdgeCount { get { return _edgeCount; } }

    public GridGraph() {
        _coordIndex = new Dictionary<XY, GridVertex<T, U>>();
        _valueIndex = new Dictionary<T, GridVertex<T, U>>();
        _edgeCount = 0;
    }

    public GridVertex<T, U> AddVertex(XY coord, T value) {
        GridVertex<T, U> newVertex = new GridVertex<T, U>(coord, value);

        _coordIndex[newVertex.Coord] = newVertex;
        _valueIndex[newVertex.Value] = newVertex;

        UpdateNeighbors(newVertex);

        return newVertex;
    }

    public GridVertex<T, U> GetVertexByCoord(XY coord) {
        return (_coordIndex.ContainsKey(coord) ? _coordIndex[coord] : null);
    }

    public GridVertex<T, U> GetVertexByValue(T value) {
        return (_valueIndex.ContainsKey(value) ? _valueIndex[value] : null);
    }

    public GridEdge<T, U> AddEdge(GridVertex<T, U> from, GridVertex<T, U> to, U edgeValue) {
        GridEdge<T, U> newEdge = new GridEdge<T, U>(from, to, edgeValue);
        from.Edges[newEdge.Direction] = newEdge;
        _edgeCount++;

        return newEdge;
    }

    public XY GetCoordForNeighbor(GridVertex<T, U> from, GridDirection direction) {
        XY step = null;

        if(direction == GridDirection.N) step = new XY(0, 1);
        if(direction == GridDirection.E) step = new XY(1, 0);
        if(direction == GridDirection.S) step = new XY(0, -1);
        if(direction == GridDirection.W) step = new XY(-1, 0);

        return from.Coord + step;
    }

    public Dictionary<GridDirection, GridVertex<T, U>> GetNeighbors(GridVertex<T, U> vertex) {
        Dictionary<GridDirection, GridVertex<T, U>> neighbors = new Dictionary<GridDirection, GridVertex<T, U>>();

        foreach(GridDirection direction in GridDirection.All) {
            XY coord = GetCoordForNeighbor(vertex, direction);

            if(_coordIndex.ContainsKey(coord))
                neighbors[direction] = _coordIndex[coord];
        }

        return neighbors;
    }

    public IEnumerable<GridVertex<T, U>> BreadthFirstSearch(GridVertex<T, U> root) {
        Dictionary<GridVertex<T, U>, SearchColor> visited = new Dictionary<GridVertex<T, U>, SearchColor>();

        foreach(GridVertex<T, U> vertex in _coordIndex.Values)
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

    private void UpdateNeighbors(GridVertex<T, U> vertex) {
        Dictionary<GridDirection, GridVertex<T, U>> neighbors = GetNeighbors(vertex);

        foreach(KeyValuePair<GridDirection, GridVertex<T, U>> entry in neighbors) {
            GridDirection direction = entry.Key;
            GridVertex<T, U> neighbor = entry.Value;

            vertex.Neighbors[direction] = neighbor;
            neighbor.Neighbors[direction.Reverse()] = vertex;
        }
    }
}
