using UnityEngine;
using System.Collections;

public class MapTile {
    public int X { get; private set; }
    public int Y { get; private set; }

    public int SpriteIndex { get; private set; }

    public MapTile(int x, int y, int spriteIndex) {
        X = x;
        Y = y;
        SpriteIndex = spriteIndex;
    }

    public override string ToString() {
        return string.Format("MapTile ({0}, {1})", X, Y);
    }
}
