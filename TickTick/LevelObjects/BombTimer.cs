using Engine;
using Microsoft.Xna.Framework;
using System;

namespace TickTick.LevelObjects
{
    class BombTimer : GameObjectList
    {
        double timeLeft;
        public bool Running { get; set; }
        public float Multiplier { get; set; }


        TextGameObject label;
        
        public bool HasPassed
        {
            get
            {
                return timeLeft <= 0;
            }
        }
        public BombTimer()
        {
            localPosition = new Vector2(20, 20);
            SpriteGameObject background = new SpriteGameObject("Sprites/UI/spr_timer", TickTick.Depth_UIBackground);
            AddChild(background);
            label = new TextGameObject("Fonts/Mainfont", TickTick.Depth_UIForeground,
            Color.Yellow, TextGameObject.Alignment.Center);
            label.LocalPosition = new Vector2(50, 25);
            AddChild(label);
            Reset();
        }
        public override void Update(GameTime gameTime)
        {
            if (!Running)
                return;

            //decrease the timer
            if(!HasPassed)
            {
                timeLeft -= gameTime.ElapsedGameTime.TotalSeconds * Multiplier;
            }

            int secondsLeft = (int)Math.Ceiling(timeLeft);
            label.Text = CreateTimeString(secondsLeft);

            if (secondsLeft <= 10 && secondsLeft % 2 == 0)
                label.Color = Color.Red;
            else
                label.Color = Color.Yellow;
        }
        public override void Reset()
        {
            base.Reset();
            Running = true;
            timeLeft = 30f;
            Multiplier = 1f;
        }
        string CreateTimeString(int secondsLeft)
        {
            return (secondsLeft / 60).ToString().PadLeft(2, '0')
            + ":" +(secondsLeft % 60).ToString().PadLeft(2, '0');
        }
    }
}
