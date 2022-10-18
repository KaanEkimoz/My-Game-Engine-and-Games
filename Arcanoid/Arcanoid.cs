using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Arcanoid
{
    class Arcanoid : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        InputHelper inputHelper;
        static GameWorld gameWorld;
        /// <summary>
        /// The width and height of the game world, in game units.
        /// </summary>
        Point worldSize;

        /// <summary>
        /// The width and height of the window, in pixels.
        /// </summary>
        Point windowSize;

        /// <summary>
        /// A matrix used for scaling the game world so that it fits inside the window.
        /// </summary>
        Matrix spriteScale;

        public Arcanoid()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            inputHelper = new InputHelper();
            Random = new Random();
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            gameWorld = new GameWorld(Content);
            ScreenSize = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            // set the world size to the width and height of that sprite
            worldSize = new Point(gameWorld.background.Width, gameWorld.background.Height);

            // set the window size, and calculate how the game world should be scaled
            windowSize = new Point(1024, 768);
            FullScreen = false;
        }
        protected override void Update(GameTime gameTime)
        {
            inputHelper.Update();
            gameWorld.HandleInput(inputHelper);
            if (inputHelper.KeyPressed(Keys.Escape))
                Exit();

            if (inputHelper.KeyPressed(Keys.F5))
                FullScreen = !FullScreen;
            gameWorld.Update(gameTime);

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, spriteScale);
            GraphicsDevice.Clear(Color.Black);
            gameWorld.Draw(gameTime, _spriteBatch);
            base.Draw(gameTime);
            _spriteBatch.End();
        }
        public static Vector2 ScreenSize { get; private set; }
        public static Random Random { get; private set; }
        public static GameWorld GameWorld
        {
            get { return gameWorld; }
        }

        #region Screen Settings
        void ApplyResolutionSettings(bool fullScreen)
        {
            // make the game full-screen or not
            _graphics.IsFullScreen = fullScreen;

            // get the size of the screen to use: either the window size or the full screen size
            Point screenSize;
            if (fullScreen)
                screenSize = new Point(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            else
                screenSize = windowSize;

            // scale the window to the desired size
            _graphics.PreferredBackBufferWidth = screenSize.X;
            _graphics.PreferredBackBufferHeight = screenSize.Y;

            _graphics.ApplyChanges();

            // calculate and set the viewport to use
            GraphicsDevice.Viewport = CalculateViewport(screenSize);

            // calculate how the graphics should be scaled, so that the game world fits inside the window
            spriteScale = Matrix.CreateScale((float)GraphicsDevice.Viewport.Width / worldSize.X, (float)GraphicsDevice.Viewport.Height / worldSize.Y, 1);
        }

        /// <summary>
        /// Calculates and returns the viewport to use, so that the game world fits on the screen while preserving its aspect ratio.
        /// </summary>
        /// <param name="windowSize">The size of the screen on which the world should be drawn.</param>
        /// <returns>A Viewport object that will show the game world as large as possible while preserving its aspect ratio.</returns>
        Viewport CalculateViewport(Point windowSize)
        {
            // create a Viewport object
            Viewport viewport = new Viewport();

            // calculate the two aspect ratios
            float gameAspectRatio = (float)worldSize.X / worldSize.Y;
            float windowAspectRatio = (float)windowSize.X / windowSize.Y;

            // if the window is relatively wide, use the full window height
            if (windowAspectRatio > gameAspectRatio)
            {
                viewport.Width = (int)(windowSize.Y * gameAspectRatio);
                viewport.Height = windowSize.Y;
            }
            // if the window is relatively high, use the full window width
            else
            {
                viewport.Width = windowSize.X;
                viewport.Height = (int)(windowSize.X / gameAspectRatio);
            }

            // calculate and store the top-left corner of the viewport
            viewport.X = (windowSize.X - viewport.Width) / 2;
            viewport.Y = (windowSize.Y - viewport.Height) / 2;

            return viewport;
        }
        /// <summary>
        /// Gets or sets whether the game is running in full-screen mode.
        /// </summary>
        bool FullScreen
        {
            get { return _graphics.IsFullScreen; }
            set { ApplyResolutionSettings(value); }
        }

        /// <summary>
        /// Converts a position in screen coordinates to a position in world coordinates.
        /// </summary>
        /// <param name="screenPosition">A position in screen coordinates.</param>
        /// <returns>The corresponding position in world coordinates.</returns>
        Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            Vector2 viewportTopLeft = new Vector2(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y);
            float screenToWorldScale = worldSize.X / (float)GraphicsDevice.Viewport.Width;
            return (screenPosition - viewportTopLeft) * screenToWorldScale;
        }
        #endregion

    }
}
