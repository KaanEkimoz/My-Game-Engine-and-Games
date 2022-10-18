using Engine;
using Engine.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace PenguinPairs.GameStates
{
    class OptionsMenuState : GameState
    {
        //back button
        Button backButton;

        Switch hintSwitch;

        Slider bgVolumeSlider;

        Scrollbar testScrollbar;
        public OptionsMenuState()
        {
            SpriteGameObject optionsBg = new SpriteGameObject("Sprites/spr_background_options");
            gameObjects.AddChild(optionsBg);

            //adding back button
            backButton = new Button("Sprites/UI/spr_button_back");
            backButton.LocalPosition = new Vector2(415, 720);
            gameObjects.AddChild(backButton);

            //adding hint switch and initialize the first value
            hintSwitch = new Switch("Sprites/UI/spr_switch@2");
            hintSwitch.LocalPosition = new Vector2(650, 340);
            gameObjects.AddChild(hintSwitch);
            hintSwitch.Selected = PenguinPairs.HintsEnabled;

            // add a slider to control the background music volume and assigns the value to bg music
            bgVolumeSlider = new Slider(0,1,8,"Sprites/UI/spr_slider_bar","Sprites/UI/spr_slider_button");
            bgVolumeSlider.LocalPosition = new Vector2(650, 500);
            gameObjects.AddChild(bgVolumeSlider);
            bgVolumeSlider.Value = MediaPlayer.Volume;

            /*testScrollbar = new Scrollbar(0, 1, 8, "Sprites/UI/spr_scrollbar_bar", "Sprites/UI/spr_scrollbar_button");
            testScrollbar.LocalPosition = new Vector2(650, 640);
            gameObjects.AddChild(testScrollbar);*/


            TextGameObject hintsText = new TextGameObject("Fonts/MenuFont", Color.DarkBlue);
            hintsText.Text = "Hints";
            hintsText.LocalPosition = new Vector2(150, 340);
            gameObjects.AddChild(hintsText);

            //text for music volume
            TextGameObject musicVolumeText = new TextGameObject("Fonts/MenuFont", Color.DarkBlue);
            musicVolumeText.Text = "Music Volume";
            musicVolumeText.LocalPosition = new Vector2(150, 480);
            gameObjects.AddChild(musicVolumeText);
        }
        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);
            if (backButton.Pressed)
                ExtendedGame.GameStateManager.SwitchTo(PenguinPairs.StateName_Title);
            if (bgVolumeSlider.ValueChanged)
                MediaPlayer.Volume = bgVolumeSlider.Value;
            if (hintSwitch.Pressed)
                PenguinPairs.HintsEnabled = hintSwitch.Selected;
        }
    }
}
