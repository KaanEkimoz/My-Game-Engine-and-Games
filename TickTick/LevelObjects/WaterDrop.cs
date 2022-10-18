using System;
using Microsoft.Xna.Framework;
using Engine;

namespace TickTick.LevelObjects
{
    class WaterDrop : SpriteGameObject
    {
        protected float bounce;
        protected Level level;
        protected Vector2 startPos;
        public WaterDrop(Level level, Vector2 startPos) : base("Sprites/LevelObjects/spr_water", TickTick.Depth_LevelObjects)
        {
            this.startPos = startPos;
            this.level = level;
            SetOriginToCenter();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            double t = gameTime.TotalGameTime.TotalSeconds * 3.0f + localPosition.X;
            bounce = (float)Math.Sin(t) * 0.2f;
            localPosition.Y += bounce;

            if (Visible && HasPixelPreciseCollision(level.Player) && level.Player.CanCollideWithObjects)
            {
                Visible = false;
                ExtendedGame.AssetManager.PlaySoundEffect("snd_watercollected");
            }  
        }
        public override void Reset()
        {
            base.Reset();
            Visible = true;
            localPosition = startPos;
        }

    }
}
