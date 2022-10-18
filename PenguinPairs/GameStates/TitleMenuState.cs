using Engine;
using Engine.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PenguinPairs.GameStates
{
    class TitleMenuState : GameState
    {
        //main ui buttons at center of the title menu
        Button playButton, optionsButton, helpButton;

        
        public TitleMenuState()
        {
            SpriteGameObject titleScreen = new SpriteGameObject("Sprites/spr_titlescreen");
            gameObjects.AddChild(titleScreen);

            //adding play button
            playButton = new Button("Sprites/UI/spr_button_play");
            playButton.LocalPosition = new Vector2(415, 540);
            gameObjects.AddChild(playButton);

            //adding options button
            optionsButton = new Button("Sprites/UI/spr_button_options");
            optionsButton.LocalPosition = new Vector2(415, 650);
            gameObjects.AddChild(optionsButton);

            //adding help button
            helpButton = new Button("Sprites/UI/spr_button_help");
            helpButton.LocalPosition = new Vector2(415, 760);
            gameObjects.AddChild(helpButton);
        }
        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);

            if (playButton.Pressed)
                ExtendedGame.GameStateManager.SwitchTo(PenguinPairs.StateName_LevelSelect);
            if (optionsButton.Pressed)
                ExtendedGame.GameStateManager.SwitchTo(PenguinPairs.StateName_Options);
            if (helpButton.Pressed)
                ExtendedGame.GameStateManager.SwitchTo(PenguinPairs.StateName_Help);
        }
    }
}
