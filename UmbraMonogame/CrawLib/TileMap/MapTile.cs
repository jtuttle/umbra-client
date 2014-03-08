using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CrawLib.TileMap {
    public class MapTile {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int SpriteIndex { get; private set; }

        public MapTile(int x, int y, int spriteIndex) {
            X = x;
            Y = y;
            SpriteIndex = spriteIndex;
        }
    }
}
