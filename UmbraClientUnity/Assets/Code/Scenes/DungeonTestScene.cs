using UnityEngine;
using System.Collections;

public class DungeonTestScene : MonoBehaviour {
    void Awake() {
        DungeonGenerator generator = new DungeonGenerator();
        Dungeon dungeon = generator.Generate(100);

        new DungeonVisualizer().RenderDungeon(dungeon);

        Debug.Log("vertices: " + dungeon.Graph.VertexCount);
        Debug.Log("edges: " + dungeon.Graph.EdgeCount);
    }
}
