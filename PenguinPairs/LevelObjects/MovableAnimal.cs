using Engine;
using Microsoft.Xna.Framework;

namespace PenguinPairs.LevelObjects
{
    class MovableAnimal : Animal
    {
        bool isInHole;
        Vector2 targetWorldPosition;
        Point startPosition;
        bool IsSeal { get { return AnimalIndex == 7; } }
        bool IsMultiColoredPenguin { get { return AnimalIndex == 6; } }

        const float speed = 300;
        bool IsMoving { get { return LocalPosition != targetWorldPosition; } }


        public MovableAnimal(Level level,Point gridPosition,int animalIndex)
            : base(level ,gridPosition,GetSpriteName(false), animalIndex)
        {
            targetWorldPosition = LocalPosition;
            startPosition = gridPosition;
        }

        public int AnimalIndex { get { return SheetIndex; } }

        static string GetSpriteName(bool isInHole)
        {
            if (isInHole)
                return "Sprites/LevelObjects/spr_penguin_boxed@8";
            return "Sprites/LevelObjects/spr_penguin@8";
        }

        public bool IsInHole
        {
            get { return isInHole; }
            private set
            {
                isInHole = value;
                sprite = new SpriteSheet(GetSpriteName(isInHole), AnimalIndex);
            }
        }
        bool IsPairWith(MovableAnimal other)
        {
            if (IsSeal || other.IsSeal)
                return false;
            if (IsMultiColoredPenguin || other.IsMultiColoredPenguin)
                return true;

            return this.AnimalIndex == other.AnimalIndex;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsMoving && Vector2.Distance(LocalPosition, targetWorldPosition)
            < speed * gameTime.ElapsedGameTime.TotalSeconds)
            {
                ApplyCurrentPosition();
            }
        }
        public override void HandleInput(InputHelper inputHelper)
        {
            if (Visible && BoundingBox.Contains(inputHelper.MousePositionWorld)
            && inputHelper.MouseLeftButtonPressed())
            {
                level.SelectAnimal(this);
            }
        }

        public void TryMoveInDirection(Point direction)
        {
            if (!CanMoveInDirection(direction))
                return;
            level.RemoveAnimalFromGrid(currentGridPosition);
            while (CanMoveInDirection(direction))
                currentGridPosition += direction;

            targetWorldPosition = level.GetCellPosition(currentGridPosition.X, currentGridPosition.Y);
            Vector2 dir = targetWorldPosition - LocalPosition;
            dir.Normalize();
            velocity = dir * speed;
        }

        public bool CanMoveInDirection(Point direction)
        {
            if (IsMoving || isInHole || !Visible)
                return false;

            Tile.Type tileType = level.GetTileType(currentGridPosition);
            if (tileType == Tile.Type.Empty || tileType == Tile.Type.Hole)
                return false;

            Animal otherAnimal = level.GetAnimal(currentGridPosition);
            if (otherAnimal != null && otherAnimal != this)
                return false;

            Point nextPosition = currentGridPosition + direction;
            Tile.Type nextTileType = level.GetTileType(nextPosition);
            if (nextTileType == Tile.Type.Wall)
                return false;

            Animal nextAnimal = level.GetAnimal(nextPosition);

            if (nextAnimal is MovableAnimal && !IsPairWith((MovableAnimal)nextAnimal))
                return false;

            return true;
        }
        protected override void ApplyCurrentPosition()
        {
            // set the position in the game world
            LocalPosition = level.GetCellPosition(currentGridPosition.X, currentGridPosition.Y);

            // stop moving
            velocity = Vector2.Zero;

            // If the current tile already contains another animal, then both animals should disappear!
            Animal otherAnimal = level.GetAnimal(currentGridPosition);
            if (otherAnimal != null)
            {
                level.RemoveAnimalFromGrid(currentGridPosition);
                Visible = false;
                otherAnimal.Visible = false;

                // if the other animal matches, notify the game that we've made a pair
                if (otherAnimal is MovableAnimal)
                {
                    MovableAnimal other = (MovableAnimal)otherAnimal;
                    level.PairFound(this, other);
                }
                // otherwise, the animal is touching a shark: play a sound
                else
                    ExtendedGame.AssetManager.PlaySoundEffect("Sounds/snd_eat");

                return;
            }

            // if the current tile has the "empty" type, let the animal die
            Tile.Type tileType = level.GetTileType(currentGridPosition);
            if (tileType == Tile.Type.Empty)
            {
                // the animal has fallen into the water: play a sound
                ExtendedGame.AssetManager.PlaySoundEffect("Sounds/snd_eat");

                Visible = false;
                return;
            }

            // In all other cases, the animal shouldn't disappear yet, so it's still part of the level. 
            // Add the animal to the level's grid.
            level.AddAnimalToGrid(this, currentGridPosition);

            // if the current tile is a hole, mark this animal as "inside a hole"
            if (tileType == Tile.Type.Hole)
                IsInHole = true;
        }
        public override void Reset()
        {
            currentGridPosition = startPosition;
            IsInHole = false;
            base.Reset();
            targetWorldPosition = LocalPosition;
        }
    }
}
