using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    public List<MapTile> GetMapArea(XY bottomLeft, XY topRight) {
        List<MapTile> mapTiles = new List<MapTile>();

        for(int y = bottomLeft.Y; y <= topRight.Y; y++) {
            for(int x = bottomLeft.X; x <= topRight.X; x++)
                mapTiles.Add(_mapTiles[x, y]);
        }

        return mapTiles;
    }

    public static Map CreateFake(int width, int height) {
        Map map = new Map(width, height);

        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
                int sprite = ((x == 0 || x == width - 1 || y == 0 || y == height - 1) ? 1 : 0);
                map.AddMapTile(x, y, sprite);
            }
        }

        return map;
    }
}
