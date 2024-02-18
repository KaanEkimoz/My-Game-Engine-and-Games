using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Painter
{
    internal class Cannon : ThreeColorGameObject
    {
        private readonly Texture2D _cannonBarrel;
        private readonly Vector2 _barrelOrigin;
        private float _barrelRotation; // for member variables of type float, the compiler automatically assigns a default value of 0
        private readonly bool _calculateAngle;
        
        public Cannon(ContentManager content) : base(content,"spr_cannon_red","spr_cannon_green","spr_cannon_blue")
        {
            _cannonBarrel = content.Load<Texture2D>("spr_cannon_barrel");
            _barrelOrigin = new Vector2(_cannonBarrel.Height, _cannonBarrel.Height) / 2;
            position = new Vector2(72, 405);
            _calculateAngle = true;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_cannonBarrel, position, null, Color.White, _barrelRotation, _barrelOrigin, 1.0f, SpriteEffects.None, 0);
            base.Draw(gameTime,spriteBatch);

        }
        public override void HandleInput(InputHelper inputHelper)
        {
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
            if (_calculateAngle)
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
                float opposite = (float)Math.Sin(_barrelRotation) * _cannonBarrel.Width * 0.75f;
                float adjacent = (float)Math.Cos(_barrelRotation) * _cannonBarrel.Width * 0.75f;
                return position + new Vector2(adjacent, opposite);
            }
        }
        public override void Reset()
        {
            base.Reset();
            _barrelRotation = 0.0f;
        }
        public float Angle
        {
            get
            {
                return _barrelRotation;
            }
            set
            {
                _barrelRotation = value;
            }
        }
    }
    
}
