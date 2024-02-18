﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;

namespace Painter
{
    class GameWorld
    {
        //Assets
        Texture2D background;
        Texture2D livesSprite;
        Texture2D gameOver;
        Texture2D scoreBar;
        Song backgroundMusic;
        SpriteFont gameFont;
        
        //GameObjects
        PaintCan can1, can2, can3;
        int lives;
        Cannon cannon;
        Ball ball;
        public GameWorld(ContentManager content)
        {
            background = content.Load<Texture2D>("spr_background");
            backgroundMusic = content.Load<Song>("snd_music");
            livesSprite = content.Load<Texture2D>("spr_lives");
            gameOver = content.Load<Texture2D>("spr_gameover");
            scoreBar = content.Load<Texture2D>("spr_scorebar");
            gameFont = content.Load<SpriteFont>("PainterFont");
            MediaPlayer.Play(backgroundMusic);

            cannon = new Cannon(content);
            ball = new Ball(content);
            can1 = new PaintCan(content, 480.0f, Color.Red);
            can2 = new PaintCan(content, 610.0f, Color.Green);
            can3 = new PaintCan(content, 740.0f, Color.Blue);
            lives = 5;
            Score = 0;
        }
        public void Update(GameTime gameTime)
        {
            if(IsGameOver)
                return;

            ball.Update(gameTime);
            can1.Update(gameTime);
            can2.Update(gameTime);
            can3.Update(gameTime);
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            ball.Draw(gameTime, spriteBatch);
            cannon.Draw(gameTime, spriteBatch);
            can1.Draw(gameTime, spriteBatch);
            can2.Draw(gameTime, spriteBatch);
            can3.Draw(gameTime, spriteBatch);

            for(int i = 0;i<lives;i++)
            {
                spriteBatch.Draw(livesSprite, new Vector2(i * livesSprite.Width + 15, 60), Color.White);
            }
            if (IsGameOver)
            {
                spriteBatch.Draw(gameOver,
                new Vector2(Painter.ScreenSize.X - gameOver.Width,
                Painter.ScreenSize.Y - gameOver.Height) / 2,
                Color.White);
            }
            spriteBatch.Draw(scoreBar, new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(gameFont, "Score: " + Score, new Vector2(20, 18), Color.White);
            spriteBatch.End();
        }
        public void HandleInput(InputHelper inputHelper)
        {
            if (!IsGameOver)
            {
                cannon.HandleInput(inputHelper);
                ball.HandleInput(inputHelper);
            }
            else if(inputHelper.KeyPressed(Keys.Space))
            {
                Reset();
            }
        }
        public bool IsOutsideWorld(Vector2 position)
        {
            return position.X < 0 || position.X > Painter.ScreenSize.X
            || position.Y > Painter.ScreenSize.Y;
        }
        void Reset()
        {
            lives = 5;
            Score = 0;
            cannon.Reset();
            ball.Reset();
            can1.Reset();
            can2.Reset();
            can3.Reset();
        }
        public void LoseLife()
        {
            lives--;
        }
        public Cannon Cannon
        {
            get { return cannon; }
        }
        public Ball Ball 
        { 
            get { return ball; } 
        }
        bool IsGameOver
        {
            get { return lives <= 0; }
        }
        public int Score 
        {   
            get; 
            set; 
        }

    }
    
}
