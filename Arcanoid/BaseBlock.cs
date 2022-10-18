using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arcanoid
{
    class BaseBlock
    {
        protected Texture2D blockSprite;
        protected int maxlives;
        protected bool isAlive;
        protected int currentlives;
        protected Vector2 origin, blockPosition;

        public BaseBlock(ContentManager Content, string blockSpriteName)
        {
            blockSprite = Content.Load<Texture2D>(blockSpriteName);
            origin = new Vector2(blockSprite.Width / 2, blockSprite.Height / 2);
            Reset();
        }
        public virtual void Update(GameTime gameTime)
        {
            if (BoundingBox.Intersects(Arcanoid.GameWorld.Ball.BoundingBox))
            {
                isAlive = false;
            }
        }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(isAlive)
            {
                spriteBatch.Draw(blockSprite, blockPosition, null, Color.White, 0f, origin, 1.0f, SpriteEffects.None, 0);
            }     
        }
        public virtual void HandleInput(InputHelper inputHelper)
        {
        }
        public virtual void Reset()
        {
            isAlive = true;
        }
        public Vector2 Position
        {
            get { return blockPosition; }
        }
        public Rectangle BoundingBox
        {
            get
            {
                Rectangle spriteBounds = blockSprite.Bounds;
                spriteBounds.Offset(Position - origin);
                return spriteBounds;
            }
        }
    }
}
