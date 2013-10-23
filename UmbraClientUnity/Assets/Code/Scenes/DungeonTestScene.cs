using UnityEngine;
using System.Collections;

public class DungeonTestScene : MonoBehaviour {
    public virtual void Awake() {
        DungeonGenerator generator = new DungeonGenerator();
        Dungeon dungeon = generator.Generate(100);

        new DungeonVisualizer().RenderDungeon(dungeon);

        Debug.Log("nodes: " + dungeon.Graph.NodeCount);
        Debug.Log("edges: " + dungeon.Graph.EdgeCount);
    }
}
