using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arcanoid
{
    class PlayerBoard
    {
        Texture2D playerBoardSprite;
        Vector2 position, origin, velocity, startPosition;
        bool moveRight;
        bool moveLeft;
        public PlayerBoard(ContentManager Content)
        {
            playerBoardSprite = Content.Load<Texture2D>("sprites/player_board");
            origin = new Vector2(playerBoardSprite.Width / 2, playerBoardSprite.Height / 2);
            startPosition = new Vector2(512, 680);
            velocity = new Vector2(250f, 0f);
            Reset();
        }
        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(moveRight)
            {
                position.X -= velocity.X * dt;
                position.X = MathHelper.Clamp(position.X, 0f, 1024f);
            }
            else if (moveLeft)
            {
                position.X += velocity.X * dt;
                position.X = MathHelper.Clamp(position.X, 0f, 1024f);
            }
            

        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(playerBoardSprite, position,null,Color.White, 0f, origin, 1.0f, SpriteEffects.None,0);
        }
        private void Reset()
        {
            position = startPosition;
            moveRight = false;
            moveLeft = false;
        }
        public void HandleInput(InputHelper inputHelper)
        {
            if(inputHelper.KeyDown(Keys.A))
            {
                moveRight = true;
                moveLeft = false;
            }
            else if (inputHelper.KeyDown(Keys.D))
            {
                moveLeft = true;
                moveRight = false;
            }
            else
            {
                moveRight = false;
                moveLeft = false;
            }
            if (inputHelper.KeyDown(Keys.A) && inputHelper.KeyDown(Keys.D))
            {
                moveRight = !moveRight;
                moveLeft = !moveLeft;
            }
            
        }
        public Vector2 Position 
        {
            get 
            {
                return position;
            } 
        }
        
    }
}
