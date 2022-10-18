using Engine;
using Microsoft.Xna.Framework;

namespace PenguinPairs.LevelObjects
{
    abstract class Animal : SpriteGameObject
    {
        protected Level level;
        protected Point currentGridPosition;
        protected Animal(Level level,Point gridPosition,string spriteName, int sheetIndex = 0) : base(spriteName, sheetIndex)
        {
            this.level = level;
            currentGridPosition = gridPosition;
            ApplyCurrentPosition();
        }
        protected virtual void ApplyCurrentPosition()
        {
            level.AddAnimalToGrid(this, currentGridPosition);
            LocalPosition = level.GetCellPosition(currentGridPosition.X, currentGridPosition.Y);
        }
        public override void Reset()
        {
            base.Reset();
            Visible = true;
            ApplyCurrentPosition();
        }
    }
}
