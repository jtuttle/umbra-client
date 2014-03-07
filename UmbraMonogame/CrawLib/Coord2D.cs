using System;
using System.Collections;
using System.Collections.Generic;

public class Coord2D {
    public int X { get; private set; }
    public int Y { get; private set; }

    public List<Coord2D> Neighbors {
        get {
            return new List<Coord2D>() {
                new Coord2D(X, Y + 1),
                new Coord2D(X - 1, Y),
                new Coord2D(X + 1, Y),
                new Coord2D(X, Y - 1)
            };
        }
    }

    public Coord2D(int x, int y) {
        X = x;
        Y = y;
    }

    public Coord2D(Hashtable json) {
        FromJson(json);
    }

    public void FromJson(Hashtable json) {
        X = int.Parse(json["X"].ToString());
        Y = int.Parse(json["Y"].ToString());
    }

    public Hashtable ToJson() {
        Hashtable json = new Hashtable();

        json["X"] = X;
        json["Y"] = Y;

        return json;
    }

    public override string ToString() {
        return string.Format("Coord2D ({0}, {1})", X, Y);
    }

    public static Coord2D operator +(Coord2D a, Coord2D b) {
        return new Coord2D(a.X + b.X, a.Y + b.Y);
    }

    public static Coord2D operator -(Coord2D a, Coord2D b) {
        return new Coord2D(a.X - b.X, a.Y - b.Y);
    }

    public static Coord2D operator *(Coord2D a, int b) {
        return new Coord2D(a.X * b, a.Y * b);
    }

    public static Coord2D operator /(Coord2D a, int b) {
        return new Coord2D(a.X / b, a.Y / b);
    }

    public override bool Equals(Object other) {
        if(other == null) return false;

        Coord2D otherCoord2D = other as Coord2D;
        if(otherCoord2D == null) return false;

        return (X == otherCoord2D.X) && (Y == otherCoord2D.Y);
    }

    public bool Equals(Coord2D other) {
        if(other == null) return false;
        return (X == other.X) && (Y == other.Y);
    }

    public override int GetHashCode() {
        return X ^ Y;
    }
}
