using System;

namespace Painter
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Painter())
                game.Run();
        }
    }
}
