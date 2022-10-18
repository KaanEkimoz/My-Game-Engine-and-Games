using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arcanoid
{
    class BlockGrid
    {
        BaseBlock[,] grid;
        Vector2 position;
        Vector2 gridOffset;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public BlockGrid(int width, int height, ContentManager content)
        {
            gridOffset = new Vector2(115,60);
            position = gridOffset;
            Width = width;
            Height = height;
            grid = new BaseBlock[Width, Height];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    position += gridOffset + new Vector2(64 * x, 32 * y);
                    grid[x, y] = new OneHitBlock(content, position);
                    position = gridOffset;
                }
            }
        }
        public void Update(GameTime gameTime)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    grid[x, y].Update(gameTime);
                }
            }

        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    grid[x, y].Draw(gameTime,spriteBatch);
                }
            }
        }
    }
}
