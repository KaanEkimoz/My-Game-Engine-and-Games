using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System;r
using Microsoft.Xna.Framework.Audio;

namespace Painter
{
    class PaintCan : ThreeColorGameObject
    {
        Color targetColor;
        float minSpeed;
        SoundEffect soundCollect;
        public PaintCan(ContentManager Content, float positionOffsetX, Color target) : base(Content,"spr_can_red","spr_can_green","spr_can_blue")
        {
            soundCollect = Content.Load<SoundEffect>("snd_collect_points");
            minSpeed = 30;
            targetColor = target;
            position = new Vector2(positionOffsetX, -origin.Y);
            Reset();
        }
        public override void Update(GameTime gameTime)
        {
            rotation = (float)Math.Sin(position.Y / 50.0) * 0.05f;
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            minSpeed += 0.01f * dt;

            if (velocity != Vector2.Zero)
            {
                position += velocity * dt;
                if (Painter.GameWorld.IsOutsideWorld(position - origin))
                {
                    if (Color != targetColor)
                        Painter.GameWorld.LoseLife();
                    else
                    {
                        Painter.GameWorld.Score += 10;
                    }
                    Reset();
                }
            }
            else if (Painter.Random.NextDouble() < 0.01)
            {
                velocity = CalculateRandomVelocity();
                Color = CalculateRandomColor();
            }

            if (BoundingBox.Intersects(Painter.GameWorld.Ball.BoundingBox))
            {
                Color = Painter.GameWorld.Ball.Color;
                Painter.GameWorld.Ball.Reset();
            }
            if (Painter.GameWorld.IsOutsideWorld(position - origin))
            {
                // if the color is wrong, the player loses a life
                if (Color != targetColor)
                    Painter.GameWorld.LoseLife();
                // otherwise, the player earns points
                else
                {
                    Painter.GameWorld.Score += 10; // this line is new!
                    soundCollect.Play();
                }
                Reset();
            }
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime,spriteBatch);
        }
        public override void Reset()
        {
            base.Reset();
            velocity = Vector2.Zero;
            position.Y = -origin.Y;
            ResetMinSpeed();
        }
        public void ResetMinSpeed()
        {
            minSpeed = 30f;
        }
        Vector2 CalculateRandomVelocity()
        {
            return new Vector2(0.0f, (float)Painter.Random.NextDouble() * 30 + minSpeed);
        }
        Color CalculateRandomColor()
        {
            int randomval = Painter.Random.Next(3);
            if (randomval == 0)
                return Color.Red;
            else if (randomval == 1)
                return Color.Green;
            else
                return Color.Blue;
        }
    }
}
