using UnityEngine;
using System.Collections;

public class Map {
    private MapTile[,] _mapTiles;

    public Map(int width, int height) {
        _mapTiles = new MapTile[width, height];
    }

    public void AddMapTile(int x, int y, int spriteIndex) {
        _mapTiles[x, y] = new MapTile(x, y, spriteIndex);
    }

    public void AddMapTile(MapTile mapTile) {
        _mapTiles[mapTile.X, mapTile.Y] = mapTile;
    }
}
