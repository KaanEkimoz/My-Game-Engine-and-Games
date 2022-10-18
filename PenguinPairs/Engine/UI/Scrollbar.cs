using Microsoft.Xna.Framework;

namespace Engine.UI
{
    class Scrollbar : GameObjectList
    {
        SpriteGameObject background, thumb;
        float minValue, maxValue;
        float currentValue;
        float previousValue;

        // The number of pixels that the thumb block should stay away from the border.
        float padding;

        float Range { get { return maxValue - minValue; } }
        float AvailableHeight { get { return MaxLocalY - MinLocalY; } }
        float MinLocalY { get { return padding + thumb.Height / 2; } }
        float MaxLocalY { get { return background.Height - padding - thumb.Height / 2; } }
        public float Value
        {
            get { return currentValue; }
            set
            {
                // store the value
                currentValue = MathHelper.Clamp(value, minValue, maxValue);

                // calculate the new position of the foreground image
                float fraction = (currentValue - minValue) / Range;
                float newY = MinLocalY + fraction * AvailableHeight;
                thumb.LocalPosition = new Vector2(padding, newY);
            }
        }

        public Scrollbar(float minValue, float maxValue, float padding, string bgSprite, string thumbSprite)
        {
            background = new SpriteGameObject(bgSprite);
            AddChild(background);

            thumb = new SpriteGameObject(thumbSprite);
            thumb.Origin = new Vector2(0, thumb.Height / 2);
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
                float correctedY = mousePos.Y - GlobalPosition.Y - MinLocalY;
                float newFraction = correctedY / AvailableHeight;
                // convert that to a new slider value
                Value = newFraction * Range + minValue;
            }
            else if(inputHelper.MouseScrollWheelDirection() != 0)
            {
                Value = inputHelper.MouseScrollWheelDirection()* 1000 * Range + minValue;
            }
            

        }
        public bool ValueChanged
        {
            get { return currentValue != previousValue; }
        }
    }
}
