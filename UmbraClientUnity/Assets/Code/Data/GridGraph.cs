using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum GridDirection { N, E, S, W }

public class GridVertex<T> {
    public XY Coord { get; private set; }
    public T Value { get; private set; }

    public GridVertex(XY coord, T value) {
        Coord = coord;
        Value = value;
    }

    public override string ToString() {
        return "Vertex @ " + Coord;
    }
}

public class Edge<T,U> {
    public GridVertex<T> From { get; private set; }
    public GridVertex<T> To { get; private set; }
    public U Value { get; private set; }

    public Edge(GridVertex<T> from, GridVertex<T> to, U value) {
        From = from;
        To = to;
        Value = value;
    }

    public override string ToString() {
        return "Edge from " + From.ToString() + " to " + To.ToString();
    }
}

public class GridGraph<T,U> {
    private Dictionary<XY, GridVertex<T>> _vertices;
    private Dictionary<GridVertex<T>, Dictionary<GridDirection, Edge<T, U>>> _edges;

    // list of vertices in graph that have at least one unused grid edge
    public List<GridVertex<T>> OpenVertices { get; private set; }

    private int _vertexCount;
    public int VertexCount { get { return _vertexCount; } }

    private int _edgeCount;
    public int EdgeCount { get { return _edgeCount; } }

    public GridGraph() {
        _vertices = new Dictionary<XY, GridVertex<T>>();
        _edges = new Dictionary<GridVertex<T>, Dictionary<GridDirection, Edge<T,U>>>();

        OpenVertices = new List<GridVertex<T>>();
    }

    public void AddVertex(GridVertex<T> vertex) {
        _vertices[vertex.Coord] = vertex;
        _edges[vertex] = new Dictionary<GridDirection, Edge<T, U>>();
        _vertexCount++;

        OpenVertices.Add(vertex);
    }

    public void AddEdge(GridVertex<T> from, GridVertex<T> to, U edgeValue) {
        GridDirection direction = GetDirection(from, to);

        _edges[from][direction] = new Edge<T, U>(from, to, edgeValue);
        _edgeCount++;

        if(OpenEdges(from).Count == 0)
            OpenVertices.Remove(from);
    }

    public List<GridDirection> OpenEdges(GridVertex<T> vertex) {
        List<GridDirection> open = new List<GridDirection>() {
            GridDirection.N, GridDirection.E, GridDirection.S, GridDirection.W
        };

        foreach(GridDirection direction in _edges[vertex].Keys)
            open.Remove(direction);

        return open;
    }

    public XY GetCoordForNextVertex(GridVertex<T> from, GridDirection direction) {
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

    private GridDirection GetDirection(GridVertex<T> from, GridVertex<T> to) {
        XY fromCoord = from.Coord;
        XY toCoord = to.Coord;

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
