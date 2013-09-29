using UnityEngine;
using System.Collections;

using DungeonVertex = GridVertex<DungeonRoom, DungeonPath>;
using DungeonEdge = GridEdge<DungeonRoom, DungeonPath>;
using System;
using System.Collections.Generic;

public class DungeonVisualizer {
    private float _spacing = 1.5f;

    public void RenderDungeon(Dungeon dungeon) {
        foreach(DungeonVertex vertex in dungeon.Graph.BreadthFirstSearch(dungeon.Entrance)) {
            RenderRoom(vertex);

            foreach(DungeonEdge edge in vertex.Edges.Values)
                RenderEdge(edge);
        }
    }

    private void RenderRoom(DungeonVertex vertex) {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = VertexPosition(vertex);
    }

    private void RenderEdge(DungeonEdge edge) {
        GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        bool horizontal = (edge.Direction == GridDirection.E || edge.Direction == GridDirection.W);
        float rotation = (horizontal ? (90.0f * Mathf.Deg2Rad) : 0);

        cylinder.transform.localScale = new Vector3(0.1f, _spacing / 2, 0.1f);
        cylinder.transform.position = EdgePosition(edge);
        cylinder.transform.rotation = Quaternion.EulerAngles(0, 0, rotation);
    }

    private Vector3 VertexPosition(DungeonVertex vertex) {
        return new Vector3(vertex.Coord.X * _spacing, vertex.Coord.Y * _spacing, 0);
    }

    private Vector3 EdgePosition(DungeonEdge edge) {
        Vector3 vertexPos = VertexPosition(edge.From);

        GridDirection direction = edge.Direction;

        float edgeSpacing = _spacing / 8;
        float halfSpacing = _spacing / 2;

        switch(direction) {
            case GridDirection.N:
                return vertexPos + new Vector3(-edgeSpacing, halfSpacing, 0);
            case GridDirection.E:
                return vertexPos + new Vector3(halfSpacing, edgeSpacing, 0);
            case GridDirection.S:
                return vertexPos + new Vector3(edgeSpacing, -halfSpacing, 0);
            case GridDirection.W:
                return vertexPos + new Vector3(-halfSpacing, -edgeSpacing, 0);
        }

        throw new Exception("Could not determine edge position");
    }
}
