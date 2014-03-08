using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrawLib.TileMap {
    public class Map {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public MapTile[] Tiles { get; private set; }

        public Map(int width, int height) {
            Width = width;
            Height = height;

            Tiles = new MapTile[width * height];
        }
    }
}
