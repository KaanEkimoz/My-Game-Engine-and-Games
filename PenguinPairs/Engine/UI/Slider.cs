using Microsoft.Xna.Framework;

namespace Engine.UI
{
    class Slider : GameObjectList
    {
        SpriteGameObject background, thumb;
        float minValue, maxValue;
        float currentValue;
        float previousValue;

        // The number of pixels that the thumb block should stay away from the border.
        float padding;

        float Range { get { return maxValue - minValue; }}
        float AvailableWidth { get { return MaxLocalX - MinLocalX; } }
        float MinLocalX { get { return padding + thumb.Width / 2; } }
        float MaxLocalX { get { return background.Width - padding - thumb.Width / 2; } }
        public float Value
        {
            get { return currentValue; }
            set
            {
                // store the value
                currentValue = MathHelper.Clamp(value, minValue, maxValue);

                // calculate the new position of the foreground image
                float fraction = (currentValue - minValue) / Range;
                float newX = MinLocalX + fraction * AvailableWidth;
                thumb.LocalPosition = new Vector2(newX, padding);
            }
        }

        public Slider(float minValue, float maxValue, float padding ,string bgSprite, string thumbSprite)
        {

            background = new SpriteGameObject(bgSprite);
            AddChild(background);

            thumb = new SpriteGameObject(thumbSprite);
            thumb.Origin = new Vector2(thumb.Width / 2, 0);
            AddChild(thumb);

            this.minValue = minValue;
            this.maxValue = maxValue;
            this.padding = padding;
            

            

            previousValue = this.maxValue / 2;
            Value = previousValue;
        }
        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);

            if (!Visible)
                return;

            Vector2 mousePos = inputHelper.MousePositionWorld;

            previousValue = Value;
            
            if (inputHelper.MouseLeftButtonDown() && background.BoundingBox.Contains(mousePos))
            {
                // translate the mouse position to a number between 0 (left) and 1 (right)
                float correctedX = mousePos.X - GlobalPosition.X - MinLocalX;
                float newFraction = correctedX / AvailableWidth;
                // convert that to a new slider value
                Value = newFraction * Range + minValue;
            }
        }
        public bool ValueChanged
        {
            get { return currentValue != previousValue; }
        }

    }
}
