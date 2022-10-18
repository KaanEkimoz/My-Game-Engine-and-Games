using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JewelJam.Engine
{
    /// <summary>
    /// A class that manages mouse and keyboard input.
    /// </summary>
    class InputHelper
    {
        // the current and previous mouse state
        MouseState currentMouseState, previousMouseState;
        // the current and previous keyboard state
        KeyboardState currentKBState, previousKBState;

        /// <summary>
        /// Updates this InputHelper object for one frame of the game loop.
        /// This method retrievse the current the mouse and keyboard state, and stores the previous states as a backup.
        /// </summary>
        public void Update()
        {
            previousMouseState = currentMouseState;
            previousKBState = currentKBState;
            currentMouseState = Mouse.GetState();
            currentKBState = Keyboard.GetState();

            MouseLeftButtonPressed();
        }

        /// <summary>
        /// Checks and returns whether the player has started pressing a certain keyboard key in the last frame of the game loop.
        /// </summary>
        /// <param name="k">The key to check.</param>
        /// <returns>true if the given key is now pressed and was not yet pressed in the previous frame; false otherwise.</returns>
        public bool KeyPressed(Keys k)
        {
            return currentKBState.IsKeyDown(k) && previousKBState.IsKeyUp(k);
        }

        /// <summary>
        /// Checks and returns whether the player has pressed a certain keyboard key
        /// </summary>
        /// <param name="k"></param>
        /// <returns>true if the given key is now pressed</returns>
        public bool KeyDown(Keys k)
        {
            return currentKBState.IsKeyDown(k);
        }
        /// <summary>
        /// Checks and returns whether the player has started pressing the left mouse button in the last frame of the game loop.
        /// </summary>
        /// <returns>true if the left mouse button is now pressed and was not yet pressed in the previous frame; false otherwise.</returns>
        public bool MouseLeftButtonPressed()
        {
            return currentMouseState.LeftButton == ButtonState.Pressed
            && previousMouseState.LeftButton == ButtonState.Released;
        }
        /// <summary>
        /// Gets the current position of the mouse, relative to the top-left corner of the screen.
        /// </summary>
        public Vector2 MousePosition
        {
            get { return new Vector2(currentMouseState.X, currentMouseState.Y); }
        }
    }
}