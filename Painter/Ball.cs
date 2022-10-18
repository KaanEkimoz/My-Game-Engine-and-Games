using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Painter_monogame
{
    class Ball : ThreeColorGameObject
    {
        bool shooting;
        SoundEffect soundShoot;


        public Ball(ContentManager Content) : base(Content, "spr_cannon_red", "spr_cannon_green", "spr_cannon_blue")
        {
            soundShoot = Content.Load<SoundEffect>("snd_shoot_paint");
            Reset();
        }
        public override void Update(GameTime gameTime)
        {
            if (shooting)
            {
                float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
                position += velocity * dt;
                velocity.Y += 400.0f * dt;
            }
            else
            {
                Color = Painter.GameWorld.Cannon.Color;
                position = Painter.GameWorld.Cannon.BallPosition;
            }
            if (Painter.GameWorld.IsOutsideWorld(position))
            {
                Reset();
            }
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime,spriteBatch);
        }
        public override void HandleInput(InputHelper inputHelper)
        {
            if (inputHelper.MouseLeftButtonPressed() && !shooting)
            {
                shooting = true;
                soundShoot.Play();
                velocity = (inputHelper.MousePosition - Painter.GameWorld.Cannon.Position) * 1.2f;
            }
        }
        public override void Reset()
        {
            base.Reset();
            velocity = Vector2.Zero;
            position = new Vector2(65, 390);
            shooting = false;
        }
    }
}
