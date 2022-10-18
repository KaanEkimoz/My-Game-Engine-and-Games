using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Painter_monogame
{
    class Cannon : ThreeColorGameObject
    {
        Texture2D cannonBarrel;
        Vector2 barrelOrigin;
        float barrelRotation;

        // for member variables of type float, the compiler automatically assigns a default value of 0
        // In C#, the default value is 0 for numbers, false for Boolean variables, and null for class types (such as Cannon)
        bool calculateAngle;



        public Cannon(ContentManager Content) : base(Content,"spr_cannon_red","spr_cannon_green","spr_cannon_blue")
        {
            cannonBarrel = Content.Load<Texture2D>("spr_cannon_barrel");
            barrelOrigin = new Vector2(cannonBarrel.Height, cannonBarrel.Height) / 2;
            position = new Vector2(72, 405);
            calculateAngle = true;
        }

        public override void Draw(GameTime gameTime, SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(cannonBarrel, position, null, Color.White, barrelRotation, barrelOrigin, 1.0f, SpriteEffects.None, 0);
            base.Draw(gameTime,_spriteBatch);

        }
        public override void HandleInput(InputHelper inputHelper)
        {
            /*if (inputHelper.MouseLeftButtonPressed())
            {
                calculateAngle = !calculateAngle;
            }*/
            if (inputHelper.KeyPressed(Keys.R))
            {
                Color = Color.Red;
            }
            else if (inputHelper.KeyPressed(Keys.G))
            {
                Color = Color.Green;
            }
            else if (inputHelper.KeyPressed(Keys.B))
            {
                Color = Color.Blue;
            }
            if (calculateAngle)
            {
                double opposite = inputHelper.MousePosition.Y - Position.Y;
                double adjacent = inputHelper.MousePosition.X - Position.X;
                Angle = (float)Math.Atan2(opposite, adjacent);
            }
            else
            {
                Angle = 0;
            }
        }
        public Vector2 BallPosition
        {
            get
            {
                float opposite = (float)Math.Sin(barrelRotation) * cannonBarrel.Width * 0.75f;
                float adjacent = (float)Math.Cos(barrelRotation) * cannonBarrel.Width * 0.75f;
                return position + new Vector2(adjacent, opposite);
            }
        }
        public override void Reset()
        {
            base.Reset();
            barrelRotation = 0.0f;
        }
        public float Angle
        {
            get
            {
                return barrelRotation;
            }
            set
            {
                barrelRotation = value;
            }
        }
    }
    
}
