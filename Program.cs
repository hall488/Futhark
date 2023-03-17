using System;

namespace Futhark
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Futhark_Game())
                game.Run();
        }
    }
}
