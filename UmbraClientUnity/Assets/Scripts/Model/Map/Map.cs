using UnityEngine;
using System.Collections;

public class Map {
    public int Width { get; private set; }
    public int Height { get; private set; }

    private MapTile[,] _mapTiles;

    public Map(int width, int height) {
        Width = width;
        Height = height;

        _mapTiles = new MapTile[width, height];
    }

    public void AddMapTile(int x, int y, int spriteIndex) {
        _mapTiles[x, y] = new MapTile(x, y, spriteIndex);
    }

    public void AddMapTile(MapTile mapTile) {
        _mapTiles[mapTile.X, mapTile.Y] = mapTile;
    }

    public MapTile GetMapTile(int x, int y) {
        if(x < 0 || x > _mapTiles.GetLength(0) - 1 || y < 0 || y > _mapTiles.GetLength(1))
            return null;

        return _mapTiles[x, y];
    }
}
