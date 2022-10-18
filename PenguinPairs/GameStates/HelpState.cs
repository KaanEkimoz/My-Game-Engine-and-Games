using Engine;
using Engine.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PenguinPairs.GameStates
{
    class HelpState : GameState
    {
        //back button
        Button backButton;
        public HelpState()
        {
            SpriteGameObject helpBg = new SpriteGameObject("Sprites/spr_background_help");
            gameObjects.AddChild(helpBg);

            //adding back button
            backButton = new Button("Sprites/UI/spr_button_back");
            backButton.LocalPosition = new Vector2(415, 720);
            gameObjects.AddChild(backButton);
        }
        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);
            if (backButton.Pressed)
                ExtendedGame.GameStateManager.SwitchTo(PenguinPairs.StateName_Title);
        }
    }
}
