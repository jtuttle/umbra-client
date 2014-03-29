using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrawLib {
    public class TextureFrame : RectangleF {
        public TextureFrame(float x, float y, float width, float height)
            : base(x, y, width, height) {

            if(x < 0 || x > 1 || y < 0 || y > 1 || width < 0 || width > 1 || height < 0 || width > 1)
                throw new Exception("Texture Frame parameters must be between 0 and 1.");
        }
    }
}
