using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Engine
{
    abstract class GameState : IGameLoopObject
    {
        protected GameObjectList gameObjects;

        protected GameState()
        {
            gameObjects = new GameObjectList();
        }

        public virtual void Update(GameTime gameTime)
        {
            gameObjects.Update(gameTime);
        }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            gameObjects.Draw(gameTime, spriteBatch);
        }
        public virtual void HandleInput(InputHelper inputHelper)
        {
            gameObjects.HandleInput(inputHelper);
        }
        public virtual void Reset()
        {
            gameObjects.Reset();
        }
    }
}
