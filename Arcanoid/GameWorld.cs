using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arcanoid
{
    
    class GameWorld
    {
        public Texture2D background;
        PlayerBoard playerBoard;
        Ball ball;
        BlockGrid blockGrid;
        public GameWorld(ContentManager content)
        {
            background = content.Load<Texture2D>("sprites/background");
            playerBoard = new PlayerBoard(content);
            ball = new Ball(content);
            blockGrid = new BlockGrid(10,10,content);
        }
        public void Update(GameTime gameTime)
        {
            if (IsGameOver)
            {
                return;
            }
            playerBoard.Update(gameTime);
            ball.Update(gameTime);
            blockGrid.Update(gameTime);
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            playerBoard.Draw(gameTime, spriteBatch);
            ball.Draw(gameTime, spriteBatch);
            blockGrid.Draw(gameTime, spriteBatch);
        }
        public void HandleInput(InputHelper inputHelper)
        {
            if (!IsGameOver)
            {
                playerBoard.HandleInput(inputHelper);
                ball.HandleInput(inputHelper);
            }
            else if (inputHelper.KeyPressed(Keys.Space))
            {
                Reset();
            }
        }
        void Reset()
        {

        }
        bool IsGameOver
        {
            get;
        }
        public int Score
        {
            get;
            set;
        }

        public PlayerBoard PlayerBoard
        {
            get { return playerBoard; }
        }
        public Ball Ball
        {
            get { return ball; }
        }
    }
    
}
