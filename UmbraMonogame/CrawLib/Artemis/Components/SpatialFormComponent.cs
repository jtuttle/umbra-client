using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis.Interface;

namespace CrawLib.Artemis.Components {
    public class SpatialFormComponent : IComponent {
        public string SpatialFormFile { get; set; }

        public SpatialFormComponent() 
            : this(string.Empty) {

        }

        public SpatialFormComponent(string spatialFormFile) {
            SpatialFormFile = spatialFormFile;
        }
    }
}
