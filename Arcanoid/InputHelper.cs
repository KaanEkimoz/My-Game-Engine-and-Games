using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Arcanoid
{
    class InputHelper
    {
        MouseState currentMouseState, previousMouseState;
        KeyboardState currentKBState, previousKBState;
        public void Update()
        {
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
            previousKBState = currentKBState;
            currentKBState = Keyboard.GetState();

            MouseLeftButtonPressed();        
        }
        public bool KeyPressed(Keys k)
        {
            return currentKBState.IsKeyDown(k) && previousKBState.IsKeyUp(k);
        }

        public bool KeyDown(Keys k)
        {
            return currentKBState.IsKeyDown(k);
        }

        public bool MouseLeftButtonPressed()
        {
            return currentMouseState.LeftButton == ButtonState.Pressed
            && previousMouseState.LeftButton == ButtonState.Released;
        }
        public Vector2 MousePosition
        {
            get { return new Vector2(currentMouseState.X, currentMouseState.Y); }
        }
    }
    

}
