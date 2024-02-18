using Engine;
using Engine.UI;
using Microsoft.Xna.Framework;

namespace PenguinPairs.GameStates
{
    class LevelMenuState : GameState
    {
        Button backButton;

        // An array of extra references to the level buttons. 
        LevelButton[] levelButtons;
        public LevelMenuState()
        {
            SpriteGameObject levelSelectBg = new SpriteGameObject("Sprites/spr_background_levelselect");
            gameObjects.AddChild(levelSelectBg);

            //adding back button
            backButton = new Button("Sprites/UI/spr_button_back");
            backButton.LocalPosition = new Vector2(415, 720);
            gameObjects.AddChild(backButton);

            int numberOfLevels = 12;
            levelButtons = new LevelButton[numberOfLevels];

            Vector2 gridOffset = new Vector2(155, 230);
            const int buttonsPerRow = 5;
            const int spaceBetweenColumns = 30;
            const int spaceBetweenRows = 5;

            for (int i = 0; i < numberOfLevels; i++)
            {
                LevelButton levelButton = new LevelButton(i + 1, PenguinPairs.Progress[i],this);
                int row = i / buttonsPerRow;
                int column = i % buttonsPerRow;
                levelButton.LocalPosition = gridOffset + new Vector2(column * (levelButton.Width + spaceBetweenColumns), row * (levelButton.Height + spaceBetweenRows));
                gameObjects.AddChild(levelButton);
                levelButtons[i] = levelButton;
            }
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            foreach (LevelButton button in levelButtons)
            {
                if (button.Status != PenguinPairs.GetLevelStatus(button.LevelIndex))
                    button.Status = PenguinPairs.GetLevelStatus(button.LevelIndex);
            }
        }
        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);
            if (backButton.Pressed)
                ExtendedGame.GameStateManager.SwitchTo(PenguinPairs.StateName_Title);

            // if a (non-locked) level button has been pressed, go to that level
            foreach (LevelButton button in levelButtons)
            {
                if (button.Pressed && button.Status != LevelStatus.Locked)
                {
                    // go to the playing state
                    ExtendedGame.GameStateManager.SwitchTo(PenguinPairs.StateName_Playing);

                    // load the correct level
                    PlayingState playingState = (PlayingState)ExtendedGame.GameStateManager.GetGameState(PenguinPairs.StateName_Playing);
                    playingState.LoadLevel(button.LevelIndex);

                    return;
                }
            }
        }
    }
}
