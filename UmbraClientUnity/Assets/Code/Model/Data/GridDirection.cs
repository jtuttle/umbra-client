using System.Collections.Generic;
using System;

public class GridDirection {
    public static readonly GridDirection N = new GridDirection();
    public static readonly GridDirection E = new GridDirection();
    public static readonly GridDirection S = new GridDirection();
    public static readonly GridDirection W = new GridDirection();

    private GridDirection() { }

    public override string ToString() {
        if(this == N) return "N";
        if(this == E) return "E";
        if(this == S) return "S";
        if(this == W) return "W";
        throw new Exception("Unable to stringify direction");
    }

    public GridDirection Reverse() {
        if(this == N) return S;
        if(this == E) return W;
        if(this == S) return N;
        if(this == W) return E;
        throw new Exception("Unable to reverse direction");
    }

    public static List<GridDirection> All {
        get { return new List<GridDirection>() { N, E, S, W }; }
    }
}