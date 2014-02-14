using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UmbraClient.Entity.Interface {
    public interface IEntityGame {
        EntityList Entities { get; }
        World World { get; }
    }
}
