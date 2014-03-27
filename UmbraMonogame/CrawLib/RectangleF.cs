using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CrawLib {
    public class RectangleF {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public Vector2 UpperLeft { get { return new Vector2(X, Y); } }
        public Vector2 UpperRight { get { return new Vector2(X + Width, Y); } }
        public Vector2 LowerLeft { get { return new Vector2(X, Y + Height); } }
        public Vector2 LowerRight { get { return new Vector2(X + Width, Y + Height); } }

        public RectangleF(float x, float y, float width, float height) {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}
