using UnityEngine;
using System.Collections;

using DungeonVertex = GridVertex<DungeonRoom, DungeonPath>;
using DungeonEdge = GridEdge<DungeonRoom, DungeonPath>;
using System;
using System.Collections.Generic;

public class DungeonVisualizer {
    private Dungeon _dungeon;
    private GameObject _visual;

    private float _spacing = 1.5f;

    public void RenderDungeon(Dungeon dungeon) {
        _dungeon = dungeon;

        _visual = new GameObject("Dungeon Visual");

        foreach(DungeonVertex vertex in _dungeon.Graph.BreadthFirstSearch(dungeon.Entrance)) {
            Color color = Color.white;

            if(vertex == _dungeon.Entrance) color = Color.blue;
                
            RenderRoom(vertex, color);

            foreach(DungeonEdge edge in vertex.Edges.Values)
                RenderEdge(edge);
        }
    }

    private void RenderRoom(DungeonVertex vertex, Color color) {
        GameObject vertexGo = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        vertexGo.name = vertex.ToString();
        vertexGo.transform.parent = _visual.transform;
        vertexGo.transform.position = VertexPosition(vertex);
        vertexGo.renderer.material.color = color;
    }

    private void RenderEdge(DungeonEdge edge) {
        GameObject edgeGo = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        bool horizontal = (edge.Direction == GridDirection.E || edge.Direction == GridDirection.W);
        float rotation = (horizontal ? 90.0f : 0);

        edgeGo.name = edge.ToString();
        edgeGo.transform.parent = _visual.transform;
        edgeGo.transform.localScale = new Vector3(0.1f, _spacing / 2, 0.1f);
        edgeGo.transform.position = EdgePosition(edge);
        edgeGo.transform.rotation = Quaternion.Euler(0, 0, rotation);
    }

    private Vector3 VertexPosition(DungeonVertex vertex) {
        return new Vector3(vertex.Coord.X * _spacing, vertex.Coord.Y * _spacing, 0);
    }

    private Vector3 EdgePosition(DungeonEdge edge) {
        Vector3 vertexPos = VertexPosition(edge.From);

        GridDirection direction = edge.Direction;

        float edgeSpacing = _spacing / 8;
        float halfSpacing = _spacing / 2;

        Vector3 adjust = Vector3.zero;

        if(direction == GridDirection.N) adjust = new Vector3(-edgeSpacing, halfSpacing, 0);
        if(direction == GridDirection.E) adjust = new Vector3(halfSpacing, edgeSpacing, 0);
        if(direction == GridDirection.S) adjust = new Vector3(edgeSpacing, -halfSpacing, 0);
        if(direction == GridDirection.W) adjust = new Vector3(-halfSpacing, -edgeSpacing, 0);

        return vertexPos + adjust;
    }

    public void UpdatePosition(XY delta) {

    }
}
