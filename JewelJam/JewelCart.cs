using JewelJam.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JewelJam
{
    class JewelCart : SpriteGameObject
    {
        // The horizontal speed at which the cart moves.
        const float speed = 10f;

        // The distance by which the cart will be pushed back if the player scores points.
        const float pushDistance = 100f;

        // The x coordinate at which the jewel cart starts.
        float startX;

        GlitterField glitters;

        public JewelCart(Vector2 startPosition) : base("spr_jewelcart")
        {
            LocalPosition = startPosition;
            startX = startPosition.X;

            glitters = new GlitterField(sprite, 40, new Rectangle(275, 470, 430, 85));
            glitters.Parent = this;

        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            glitters.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            glitters.Draw(gameTime, spriteBatch);
        }
        public override void Reset()
        {
            velocity.X = speed;
            LocalPosition = new Vector2(startX, LocalPosition.Y);
        }

        /// <summary>
        /// Pushes the cart back by some distance, to give the player extra time.
        /// Call this method when the player scores points.
        /// </summary>
        public void PushBack()
        {
            LocalPosition = new Vector2(MathHelper.Max(LocalPosition.X - pushDistance, startX), LocalPosition.Y);
        }
        public void PushForward()
        {
            LocalPosition = new Vector2(MathHelper.Max(LocalPosition.X + pushDistance, startX), LocalPosition.Y);
        }
        public void PushBack(int pushDistance)
        {
            LocalPosition = new Vector2(MathHelper.Max(LocalPosition.X - pushDistance, startX), LocalPosition.Y);
        }
        public void PushForward(int pushDistance)
        {
            LocalPosition = new Vector2(MathHelper.Max(LocalPosition.X + pushDistance, startX), LocalPosition.Y);
        }


    }
}