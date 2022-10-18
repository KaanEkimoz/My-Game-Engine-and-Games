using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    class GameStateManager
    {
        Dictionary<string, GameState> gameStates;
        GameState currentGameState;

        public GameStateManager()
        {
            gameStates = new Dictionary<string, GameState>();
            currentGameState = null;
        }
        public void Update(GameTime gameTime)
        {
            if (currentGameState != null)
                currentGameState.Update(gameTime);
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (currentGameState != null)
                currentGameState.Draw(gameTime, spriteBatch);
        }
        public void HandleInput(InputHelper inputHelper)
        {
            if (currentGameState != null)
                currentGameState.HandleInput(inputHelper);
        }
        public void Reset()
        {
            if (currentGameState != null)
                currentGameState.Reset();
            
        }
        public void AddGameState(string name, GameState state)
        {
            gameStates[name] = state;
        }
        public GameState GetGameState(string name)
        {
            if (gameStates.ContainsKey(name))
            {
                return gameStates[name];
            }
            return null;
        }
        public void SwitchTo(string name)
        {
            if (gameStates.ContainsKey(name))
                currentGameState = gameStates[name];
        }
        string GetCurrentGameStateKey()
        {
            foreach (KeyValuePair<string, GameState> pair in gameStates)
            {
                if (pair.Value == currentGameState)
                    return pair.Key;
            }
            return "";
        }
    }
}
