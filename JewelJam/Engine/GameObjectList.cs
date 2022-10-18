using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace JewelJam.Engine
{

    /// <summary>
    /// A non-visual game object that has a list of game objects as its children.
    /// </summary>
    class GameObjectList : GameObject
    {
        /// <summary>
        /// The child objects of this game object.
        /// </summary>
        List<GameObject> children;

        /// <summary>
        /// Creates a new GameObjectList with an empty list of children.
        /// </summary>
        public GameObjectList()
        {
            children = new List<GameObject>();
        }

        /// <summary>
        /// Performs the Update method for all game objects in this GameObjectList.
        /// </summary>
        /// <param name="gameTime">An object containing information about the time that has passed in the game.</param>
        public override void Update(GameTime gameTime)
        {
            foreach (GameObject child in children)
            {
                child.Update(gameTime);
            }
        }

        /// <summary>
        /// Performs the Draw method for all game objects in this GameObjectList.
        /// </summary>
        /// <param name="gameTime">An object containing information about the time that has passed in the game.</param>
        /// <param name="spriteBatch">A sprite batch object used for drawing sprites.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (GameObject child in children)
            {
                child.Draw(gameTime, spriteBatch);
            }
        }
        /// <summary>
        /// Performs input handling for all game objects in this GameObjectList.
        /// </summary>
        /// <param name="inputHelper">An object required for handling player input.</param>
        public override void HandleInput(InputHelper inputHelper)
        {
            foreach (GameObject child in children)
            {
                child.HandleInput(inputHelper);
            }
        }

        /// <summary>
        /// Performs the Reset method for all game objects in this GameObjectList.
        /// </summary>
        public override void Reset()
        {
            foreach (GameObject child in children)
            {
                child.Reset();
            };
        }

        /// <summary>
        /// Adds a child object to this GameObjectList, and sets this GameObjectList as the parent of that object.
        /// </summary>
        /// <param name="childObject">The child game object to add.</param>
        public void AddChild(GameObject childObject)
        {
            childObject.Parent = this;
            children.Add(childObject);
        }
    }
}