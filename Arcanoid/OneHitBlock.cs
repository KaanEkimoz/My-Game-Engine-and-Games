using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arcanoid
{
    class OneHitBlock : BaseBlock
    {
        public OneHitBlock(ContentManager content, Vector2 position) : base(content, "sprites/red_block")
        {
            maxlives = 1;
            Reset();
            blockPosition = position;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(isAlive)
            {
                base.Draw(gameTime, spriteBatch);
            }
            
        }
        public override void Reset()
        {
            base.Reset();
            currentlives = maxlives;
        }
    }
}
