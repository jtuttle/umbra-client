using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using DungeonNode = GridNode<DungeonRoom, DungeonPath>;
using DungeonEdge = GridEdge<DungeonRoom, DungeonPath>;

public class DungeonVisualizer {
    private Dungeon _dungeon;
    private GameObject _visual;

    private float _spacing = 1.5f;

    public void RenderDungeon(Dungeon dungeon) {
        _dungeon = dungeon;

        _visual = new GameObject("Dungeon Visual");

        foreach(DungeonNode node in _dungeon.Graph.BreadthFirstSearch(dungeon.Entrance)) {
            Color color = Color.white;

            if(node == _dungeon.Entrance) color = Color.blue;
                
            RenderRoom(node, color);

            foreach(DungeonEdge edge in node.Edges.Values)
                RenderEdge(edge);
        }
    }

    private void RenderRoom(DungeonNode node, Color color) {
        GameObject nodeGo = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        nodeGo.name = node.ToString();
        nodeGo.transform.parent = _visual.transform;
        nodeGo.transform.position = NodePosition(node);
        nodeGo.renderer.material.color = color;
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

    private Vector3 NodePosition(DungeonNode node) {
        return new Vector3(node.Coord.X * _spacing, node.Coord.Y * _spacing, 0);
    }

    private Vector3 EdgePosition(DungeonEdge edge) {
        Vector3 nodePos = NodePosition(edge.From);

        GridDirection direction = edge.Direction;

        float edgeSpacing = _spacing / 8;
        float halfSpacing = _spacing / 2;

        Vector3 adjust = Vector3.zero;

        if(direction == GridDirection.N) adjust = new Vector3(-edgeSpacing, halfSpacing, 0);
        if(direction == GridDirection.E) adjust = new Vector3(halfSpacing, edgeSpacing, 0);
        if(direction == GridDirection.S) adjust = new Vector3(edgeSpacing, -halfSpacing, 0);
        if(direction == GridDirection.W) adjust = new Vector3(-halfSpacing, -edgeSpacing, 0);

        return nodePos + adjust;
    }

    public void UpdatePosition(XY delta) {

    }
}
