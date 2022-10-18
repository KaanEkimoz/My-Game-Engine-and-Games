using JewelJam;
using JewelJam.Engine;
using System;

namespace JewelJam
{
    class JewelJam : ExtendedGame
    {
        public static JewelJamGameWorld GameWorld
        {
            get { return (JewelJamGameWorld)gameWorld; }
        }

        [STAThread]
        static void Main()
        {
            using (var game = new JewelJam())
                game.Run();
        }
        public JewelJam()
        {
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            // initialize the game world
            gameWorld = new JewelJamGameWorld(this);

            // to re-scale the game world to the screen size, we need to set the FullScreen property again
            worldSize = GameWorld.Size;
            FullScreen = false;
        }
    }
}