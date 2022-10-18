using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arcanoid
{
    class Ball
    {
        Texture2D ballSprite;
        bool isFired;
        Vector2 position, origin, startOffset, velocity;
        public Ball(ContentManager Content)
        {
            ballSprite = Content.Load<Texture2D>("sprites/ball");
            origin = new Vector2(ballSprite.Width / 2, ballSprite.Height / 2);
            startOffset = new Vector2(0f, -50f);
            velocity = new Vector2(500f, -500f);
            Reset();
        }
        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (isFired)
            {
                position += velocity * dt;
            }
            else
            {
                position = Arcanoid.GameWorld.PlayerBoard.Position + startOffset;
            }
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ballSprite, position, null, Color.White, 0f, origin, 1.0f, SpriteEffects.None, 0);
        }

        public void HandleInput(InputHelper inputHelper)
        {
            if(inputHelper.MouseLeftButtonPressed())
            {
                isFired = true;
            }

        }
        private void Reset()
        {
            isFired = false;
        }
        public Vector2 Position
        {
            get
            {
                return position;
            }
        }
        public Rectangle BoundingBox
        {
            get
            {
                Rectangle spriteBounds = ballSprite.Bounds;
                spriteBounds.Offset(Position - origin);
                return spriteBounds;
            }
        }
    }
}
