using Microsoft.Xna.Framework;

namespace PenguinPairs.LevelObjects
{
    class Shark : Animal
    {
        public Shark(Level level, Point gridPosition)
         : base(level, gridPosition, "Sprites/LevelObjects/spr_shark")
        {
        }
    }
}
