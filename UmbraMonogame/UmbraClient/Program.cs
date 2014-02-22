#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace UmbraClient {
#if WINDOWS || LINUX
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new UmbraGameClient())
                game.Run();
        }
    }
#endif
}
