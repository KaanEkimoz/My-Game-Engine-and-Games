using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace Painter_monogame
{
     class Painter : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        InputHelper inputHelper;
        static GameWorld gameWorld;
        
        public Painter()
        {
            Content.RootDirectory = "Content";
            _graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
            inputHelper = new InputHelper();
            Random = new Random();
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            gameWorld = new GameWorld(Content);
            ScreenSize = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(Content.Load<Song>("snd_music"));

        }
        protected override void Update(GameTime gameTime)
        {
            inputHelper.Update();
            gameWorld.HandleInput(inputHelper);
            gameWorld.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            gameWorld.Draw(gameTime, _spriteBatch);
        }
        public static Vector2 ScreenSize { get; private set; }
        public static Random Random { get; private set; }

        public static GameWorld GameWorld
        {
            get { return gameWorld; }
        }
    }
}
