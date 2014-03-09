﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrawLib {
    public class RectangleF {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public RectangleF(float x, float y, float width, float height) {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}
