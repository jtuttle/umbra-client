using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum SearchColor { White, Gray, Black }

public class GridNode<T, U> {
    public XY Coord { get; private set; }
    public T Data { get; private set; }

    public Dictionary<GridDirection, GridEdge<T, U>> Edges { get; private set; }
    public Dictionary<GridDirection, GridNode<T, U>> Neighbors { get; private set; }
    
    public GridNode(XY coord, T data) {
        Coord = coord;
        Data = data;

        Edges = new Dictionary<GridDirection,GridEdge<T, U>>();
        Neighbors = new Dictionary<GridDirection, GridNode<T, U>>();
    }

    public override string ToString() {
        return "Node @ " + Coord;
    }
}

public class GridEdge<T, U> {
    public GridNode<T, U> From { get; private set; }
    public GridNode<T, U> To { get; private set; }
    public U Data { get; private set; }

    public GridEdge(GridNode<T, U> from, GridNode<T, U> to, U data) {
        From = from;
        To = to;
        Data = data;
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
    private Dictionary<XY, GridNode<T, U>> _coordIndex;
    private Dictionary<T, GridNode<T, U>> _dataIndex;
    public int NodeCount { get { return _coordIndex.Count; } }

    private int _edgeCount;
    public int EdgeCount { get { return _edgeCount; } }

    public GridGraph() {
        _coordIndex = new Dictionary<XY, GridNode<T, U>>();
        _dataIndex = new Dictionary<T, GridNode<T, U>>();
        _edgeCount = 0;
    }

    public GridNode<T, U> AddNode(XY coord, T data) {
        GridNode<T, U> newNode = new GridNode<T, U>(coord, data);

        _coordIndex[newNode.Coord] = newNode;
        _dataIndex[newNode.Data] = newNode;

        UpdateNeighbors(newNode);

        return newNode;
    }

    public GridNode<T, U> GetNodeByCoord(XY coord) {
        return (_coordIndex.ContainsKey(coord) ? _coordIndex[coord] : null);
    }

    public GridNode<T, U> GetNodeByData(T data) {
        return (_dataIndex.ContainsKey(data) ? _dataIndex[data] : null);
    }

    public GridEdge<T, U> AddEdge(GridNode<T, U> from, GridNode<T, U> to, U data) {
        GridEdge<T, U> newEdge = new GridEdge<T, U>(from, to, data);
        from.Edges[newEdge.Direction] = newEdge;
        _edgeCount++;

        return newEdge;
    }

    public XY GetCoordForNeighbor(GridNode<T, U> from, GridDirection direction) {
        XY step = null;

        if(direction == GridDirection.N) step = new XY(0, 1);
        if(direction == GridDirection.E) step = new XY(1, 0);
        if(direction == GridDirection.S) step = new XY(0, -1);
        if(direction == GridDirection.W) step = new XY(-1, 0);

        return from.Coord + step;
    }

    public Dictionary<GridDirection, GridNode<T, U>> GetNeighbors(GridNode<T, U> node) {
        Dictionary<GridDirection, GridNode<T, U>> neighbors = new Dictionary<GridDirection, GridNode<T, U>>();

        foreach(GridDirection direction in GridDirection.All) {
            XY coord = GetCoordForNeighbor(node, direction);

            if(_coordIndex.ContainsKey(coord))
                neighbors[direction] = _coordIndex[coord];
        }

        return neighbors;
    }

    public IEnumerable<GridNode<T, U>> BreadthFirstSearch(GridNode<T, U> root) {
        Dictionary<GridNode<T, U>, SearchColor> visited = new Dictionary<GridNode<T, U>, SearchColor>();

        foreach(GridNode<T, U> node in _coordIndex.Values)
            visited[node] = SearchColor.White;

        Queue<GridNode<T, U>> queue = new Queue<GridNode<T, U>>();

        queue.Enqueue(root);
        visited[root] = SearchColor.Gray;

        while(queue.Count > 0) {
            GridNode<T, U> next = queue.Dequeue();

            yield return next;

            List<GridNode<T, U>> neighbors = new List<GridNode<T, U>>(GetNeighbors(next).Values);

            foreach(GridNode<T, U> neighbor in neighbors) {
                if(visited[neighbor] == SearchColor.White) {
                    visited[neighbor] = SearchColor.Gray;
                    queue.Enqueue(neighbor);
                }
            }

            visited[next] = SearchColor.Black;
        }
    }

    private void UpdateNeighbors(GridNode<T, U> node) {
        Dictionary<GridDirection, GridNode<T, U>> neighbors = GetNeighbors(node);

        foreach(KeyValuePair<GridDirection, GridNode<T, U>> entry in neighbors) {
            GridDirection direction = entry.Key;
            GridNode<T, U> neighbor = entry.Value;

            node.Neighbors[direction] = neighbor;
            neighbor.Neighbors[direction.Reverse()] = node;
        }
    }
}
