using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis.Interface;
using CrawLib.TileMap;

namespace CrawLib.Artemis.Components {
    public class TileMapComponent : IComponent {
        public Map TileMap { get; set; }

        public TileMapComponent(Map tileMap) {
            TileMap = tileMap;
        }
    }
}
