using Engine;
using Microsoft.Xna.Framework;
using PenguinPairs.GameStates;
using System;
using System.Collections.Generic;
using System.IO;

namespace PenguinPairs
{
    class PenguinPairs : ExtendedGame
    {
        public const string StateName_Title = "title";
        public const string StateName_Help = "help";
        public const string StateName_Options = "options";
        public const string StateName_LevelSelect = "levelselect";
        public const string StateName_Playing = "playing";

        public static List<LevelStatus> Progress;

        public static int NumberOfLevels { get { return Progress.Count; } }

        public static bool HintsEnabled { get; set; }

        [STAThread]
        static void Main()
        {
            PenguinPairs game = new PenguinPairs();
            game.Run();
        }
        public PenguinPairs()
        {
            IsMouseVisible = true;
            HintsEnabled = true;
        }
        protected override void LoadContent()
        {
            base.LoadContent();

            // set a custom world and window size
            worldSize = new Point(1200, 900);
            windowSize = new Point(1024, 768);

            

            // to let these settings take effect, we need to set the FullScreen property again
            FullScreen = false;

            LoadProgress();
            AddGameStates();
            GameStateManager.SwitchTo(StateName_Title);

            AssetManager.PlaySong("Sounds/snd_music", true);
        }

        private void LoadProgress()
        {
            // prepare a list of LevelStatus values
            Progress = new List<LevelStatus>();

            // read the "levels_status" file; add a LevelStatus for each line
            StreamReader reader = new StreamReader("Content/Levels/levels_status.txt");
            string line = reader.ReadLine();
            while (line != null)
            {
                if (line == "locked")
                    Progress.Add(LevelStatus.Locked);
                else if (line == "unlocked")
                    Progress.Add(LevelStatus.Unlocked);
                else if (line == "solved")
                    Progress.Add(LevelStatus.Solved);

                //go to the next line
                line = reader.ReadLine();
            }
            reader.Close();
        }
        static void SaveProgress()
        {
            StreamWriter writer = new StreamWriter("Content/Levels/levels_status.txt");
            foreach (LevelStatus status in Progress)
            {
                if (status == LevelStatus.Locked)
                    writer.WriteLine("locked");
                else if (status == LevelStatus.Unlocked)
                    writer.WriteLine("unlocked");
                else
                    writer.WriteLine("solved");
            }
            writer.Close();
        }
        public static void MarkLevelAsSolved(int levelIndex)
        {
            SetLevelStatus(levelIndex, LevelStatus.Solved);
            if (levelIndex < NumberOfLevels && GetLevelStatus(levelIndex + 1) == LevelStatus.Locked)
                SetLevelStatus(levelIndex + 1, LevelStatus.Unlocked);
            SaveProgress();
        }
        public static LevelStatus GetLevelStatus(int levelIndex)
        {
            return Progress[levelIndex - 1];
        }
        static void SetLevelStatus(int levelIndex, LevelStatus status)
        {
            Progress[levelIndex - 1] = status;
        }
        public static void GoToNextLevel(int levelIndex)
        {
            if (levelIndex == NumberOfLevels)
                GameStateManager.SwitchTo(StateName_LevelSelect);
            else
            {
                PlayingState playingState =
                (PlayingState)GameStateManager.GetGameState(StateName_Playing);
                playingState.LoadLevel(levelIndex + 1);
            }
        }
        private void AddGameStates()
        {
            GameStateManager.AddGameState(StateName_Title, new TitleMenuState());
            GameStateManager.AddGameState(StateName_Help, new HelpState());
            GameStateManager.AddGameState(StateName_LevelSelect, new LevelMenuState());
            GameStateManager.AddGameState(StateName_Options, new OptionsMenuState());
            GameStateManager.AddGameState(StateName_Playing, new PlayingState());
        }
    }
}
