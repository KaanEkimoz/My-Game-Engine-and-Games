using Engine;
using Engine.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenguinPairs.GameStates;

namespace PenguinPairs
{
    class LevelButton : Button
    {
        /// <summary>
        /// Gets the index of the level that this button represents.
        /// </summary>
        public int LevelIndex { get; private set; }

        LevelStatus status;
        TextGameObject label;

        LevelMenuState levelMenuState;

        public LevelButton(int levelIndex, LevelStatus startStatus, LevelMenuState levelMenuState)
            : base(GetSpriteNameForStatus(startStatus))
        {
            LevelIndex = levelIndex;
            Status = startStatus;
            this.levelMenuState = levelMenuState;
            // add a label that shows the level index
            label = new TextGameObject("Fonts/ScoreFont",
                Color.Black, TextGameObject.Alignment.Center);
            label.LocalPosition = sprite.Center + new Vector2(0, 12);
            label.Parent = this;
            label.Text = levelIndex.ToString();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            label.Draw(gameTime, spriteBatch);
        }

        /// <summary>
        /// Gets or sets the status of this level button.
        /// When you change the status, the button will receive a different sprite.
        /// </summary>
        public LevelStatus Status
        {
            get { return status; }
            set
            {
                status = value;
                sprite = new SpriteSheet(GetSpriteNameForStatus(status));
                SheetIndex = (LevelIndex - 1) % sprite.NumberOfSheetElements;
            }
        }

        static string GetSpriteNameForStatus(LevelStatus status)
        {
            if (status == LevelStatus.Locked)
                return "Sprites/UI/spr_level_locked";
            if (status == LevelStatus.Unlocked)
                return "Sprites/UI/spr_level_unsolved";
            return "Sprites/UI/spr_level_solved@6";
        }
    }
}
