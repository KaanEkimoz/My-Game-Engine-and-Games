using Engine;
using Engine.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PenguinPairs.LevelObjects;

namespace PenguinPairs.GameStates
{
    class PlayingState : GameState
    {
        Button hintButton, retryButton, quitButton;
        SpriteGameObject completedOverlay;

        Level level;
        public PlayingState()
        {
            SpriteGameObject levelBg = new SpriteGameObject("Sprites/spr_background_level");
            gameObjects.AddChild(levelBg);

            //adding quit button
            quitButton = new Button("Sprites/UI/spr_button_quit");
            quitButton.LocalPosition = new Vector2(1058, 20);
            gameObjects.AddChild(quitButton);

            //adding hint button
            hintButton = new Button("Sprites/UI/spr_button_hint");
            hintButton.LocalPosition = new Vector2(916, 20);
            gameObjects.AddChild(hintButton);

            //adding retry button
            retryButton = new Button("Sprites/UI/spr_button_retry");
            retryButton.LocalPosition = new Vector2(916, 20);
            retryButton.Visible = false;
            gameObjects.AddChild(retryButton);

            //adding level finished overlay
            completedOverlay = new SpriteGameObject("Sprites/spr_level_finished");
            gameObjects.AddChild(completedOverlay);

        }
        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);
            if (completedOverlay.Visible)
            {
                if(inputHelper.MouseLeftButtonPressed())
                    PenguinPairs.GoToNextLevel(level.LevelIndex);

            }
            else
            {
                if (quitButton.Pressed)
                    ExtendedGame.GameStateManager.SwitchTo(PenguinPairs.StateName_LevelSelect);
                // if the "hint" button is pressed, show the hint arrow
                if (hintButton.Pressed)
                    level.ShowHint();
                // if the "retry" button is pressed, reset the level
                if (retryButton.Pressed)
                    level.Reset();

                if (level != null)
                    level.HandleInput(inputHelper);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (level != null)
                level.Update(gameTime);
            hintButton.Visible = PenguinPairs.HintsEnabled && !level.FirstMoveMade;
            retryButton.Visible = level.FirstMoveMade;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime,spriteBatch);
            if (level != null)
                level.Draw(gameTime,spriteBatch);
        }

        public void LoadLevel(int levelIndex)
        {
            level = new Level(levelIndex, "Content/Levels/level" + levelIndex + ".txt");
        }
        public void LevelCompleted(int levelIndex)
        {
            ExtendedGame.AssetManager.PlaySoundEffect("Sounds/snd_won");
            completedOverlay.Visible = false;
            level.Visible = false;
            PenguinPairs.MarkLevelAsSolved(levelIndex);
        }

    }
}
