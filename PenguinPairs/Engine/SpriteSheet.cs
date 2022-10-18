using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    class SpriteSheet
    {
        Texture2D sprite;
        Rectangle spriteRectangle;
        int sheetIndex, sheetColumns, sheetRows;

        public int Width { get { return sprite.Width / sheetColumns; } }
        public int Height { get { return sprite.Height / sheetRows; } }

        public Vector2 Center
        {
            get { return new Vector2(Width, Height) / 2; }
        }
        public Rectangle Bounds
        {
            get { return new Rectangle(0, 0, Width, Height); }
        }
        public int NumberOfSheetElements
        {
            get { return sheetColumns * sheetRows; }
        }
        public int SheetIndex
        {
            get { return sheetIndex; }
            set
            {
                if (value >= 0 && value < NumberOfSheetElements)
                {
                    sheetIndex = value;
                    int columnIndex = sheetIndex % sheetColumns;
                    int rowIndex = sheetIndex / sheetColumns;
                    spriteRectangle = new Rectangle(
                    columnIndex * Width, rowIndex * Height, Width, Height);
                }
            }
        }
        public SpriteSheet(string assetname, int sheetIndex = 0)
        {
            sprite = ExtendedGame.AssetManager.LoadSprite(assetname);
            sheetColumns = 1;
            sheetRows = 1;

            // see if we can extract the number of sheet elements from the assetname
            string[] assetSplit = assetname.Split('@');
            if (assetSplit.Length >= 2)
            {
                // behind the last '@' symbol, there should be a number.
                // This number can be followed by an 'x' and then another number.
                string sheetNrData = assetSplit[assetSplit.Length - 1];
                string[] columnAndRow = sheetNrData.Split('x');
                sheetColumns = int.Parse(columnAndRow[0]);
                if (columnAndRow.Length == 2)
                    sheetRows = int.Parse(columnAndRow[1]);
            }

            SheetIndex = sheetIndex;
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2 origin)
        {
            spriteBatch.Draw(sprite, position, spriteRectangle, Color.White, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
        }
    }
}
