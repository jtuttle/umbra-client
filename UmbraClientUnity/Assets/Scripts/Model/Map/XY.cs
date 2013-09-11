using UnityEngine;
using System.Collections;

public class XY {
    public int X { get; private set; }
    public int Y { get; private set; }

    public XY(int x, int y) {
        X = x;
        Y = y;
    }

    public static XY operator +(XY a, XY b) {
        return new XY(a.X + b.X, a.Y + b.Y);
    }

    public static XY operator -(XY a, XY b) {
        return new XY(a.X - b.X, a.Y - b.Y);
    }

    public static XY operator *(XY a, int b) {
        return new XY(a.X * b, a.Y * b);
    }

    public static XY operator /(XY a, int b) {
        return new XY(a.X / b, a.Y / b);
    }
}
