using System;

namespace Arcanoid
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Arcanoid())
                game.Run();
        }
    }
}
