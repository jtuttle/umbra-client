using UnityEngine;
using System.Collections;

public class DungeonTestScene : MonoBehaviour {
    void Awake() {
        DungeonGenerator generator = new DungeonGenerator();
        Dungeon dungeon = generator.Generate(10);

        new DungeonVisualizer().RenderDungeon(dungeon);
    }
}
