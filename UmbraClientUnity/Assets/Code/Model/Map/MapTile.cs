using UnityEngine;
using System.Collections;

public class MapTile {
    public int SpriteIndex { get; private set; }

    public XY Coord { get; private set; }
    public int X { get { return Coord.X; } }
    public int Y { get { return Coord.Y; } }

    public MapTile(int x, int y, int spriteIndex) {
        Coord = new XY(x, y);
        SpriteIndex = spriteIndex;
    }

    public override string ToString() {
        return string.Format("MapTile ({0}, {1})", X, Y);
    }
}
