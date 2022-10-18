using Engine;
using Microsoft.Xna.Framework;
using PenguinPairs.GameStates;
using System;
using System.Collections.Generic;
using System.IO;

namespace PenguinPairs.LevelObjects
{
    class Level : GameObjectList
    {
        const int TileWidth = 73;
        const int TileHeight = 72;
        public int LevelIndex { get; private set; }
        int targetNumberOfPairs;
        const string MovableAnimalLetters = "brgycpmx";

        MovableAnimalSelector selector;

        Tile[,] tiles;
        Animal[,] animalsOnTiles;

        SpriteGameObject hintArrow;
        VisibilityTimer hintTimer;
        PairList pairList;
        int GridWidth { get { return tiles.GetLength(0); } }
        int GridHeight { get { return tiles.GetLength(1); } }
        public bool FirstMoveMade { get; private set; }
        public Level(int levelIndex, string filename)
        {
            LevelIndex = levelIndex;
            LoadLevelFromFile(filename);
            FirstMoveMade = false;

        }

        bool IsPositionInGrid(Point gridPosition)
        {
            return gridPosition.X >= 0 && gridPosition.X < GridWidth
            && gridPosition.Y >= 0 && gridPosition.Y < GridHeight;
        }

        public Vector2 GetCellPosition(int x, int y)
        {
            return new Vector2(x * TileWidth, y * TileHeight);
        }
        
        public Animal GetAnimal(Point gridPosition)
        {
            if (!IsPositionInGrid(gridPosition))
                return null;
            return animalsOnTiles[gridPosition.X, gridPosition.Y];
        }
        public Tile.Type GetTileType(Point gridPosition)
        {
            if (!IsPositionInGrid(gridPosition))
                return Tile.Type.Empty;
            return tiles[gridPosition.X, gridPosition.Y].TileType;
        }

        private void LoadLevelFromFile(string filename)
        {
            StreamReader reader = new StreamReader(filename);
            string title = reader.ReadLine();
            string description = reader.ReadLine();
            targetNumberOfPairs = int.Parse(reader.ReadLine());
            string[] hint = reader.ReadLine().Split(' ');
            int hintX = int.Parse(hint[0]);
            int hintY = int.Parse(hint[1]);
            int hintDirection = StringToDirection(hint[2]);
            hintArrow = new SpriteGameObject("Sprites/LevelObjects/spr_arrow_hint@4", hintDirection);
            hintArrow.LocalPosition = GetCellPosition(hintX, hintY);

            hintTimer = new VisibilityTimer(hintArrow);

            

            // read the rows of the grid; keep track of the longest row
            int gridWidth = 0;

            List<string> gridRows = new List<string>();
            string line = reader.ReadLine();
            while (line != null)
            {
                if (line.Length > gridWidth)
                    gridWidth = line.Length;

                gridRows.Add(line);
                line = reader.ReadLine();
            }
            reader.Close();

            AddLevelInfoObjects(title, description);
            AddPlayingField(gridRows,gridWidth,gridRows.Count);
        }
        public void ShowHint()
        {
            hintTimer.StartVisible(2f);
        }
        void AddPlayingField(List<string> gridRows,int gridWidth, int gridHeight)
        {
            GameObjectList playingField = new GameObjectList();

            Vector2 gridSize = new Vector2(gridWidth * TileWidth, gridHeight * TileHeight);
            playingField.LocalPosition = new Vector2(600, 420) - gridSize / 2.0f;

            tiles = new Tile[gridWidth, gridHeight];
            animalsOnTiles = new Animal[gridWidth, gridHeight];

            for (int y = 0; y < gridHeight; y++)
            {
                string row = gridRows[y];
                for (int x = 0; x < gridWidth; x++)
                {
                    char symbol = ' ';
                    if (x < row.Length)
                        symbol = row[x];

                    AddTile(x, y, symbol);
                    AddAnimal(x, y, symbol);
                }

            }
            for (int y = 0; y < gridHeight; y++)
                for (int x = 0; x < gridWidth; x++)
                    playingField.AddChild(tiles[x, y]);
            for (int y = 0; y < gridHeight; y++)
                for (int x = 0; x < gridWidth; x++)
                    if (animalsOnTiles[x, y] != null)
                        playingField.AddChild(animalsOnTiles[x, y]);

            hintArrow.Visible = false;
            playingField.AddChild(hintArrow);
            selector = new MovableAnimalSelector();
            playingField.AddChild(selector);
            AddChild(playingField);

        }


        private void AddAnimal(int x, int y, char symbol)
        {
            Animal result = null;
            if (symbol == '@')
                result = new Shark(this,new Point(x,y));
            // a penguin or seal, possibly inside a hole?
            if (result == null)
            {
                int animalIndex = GetAnimalIndex(symbol);
                if (animalIndex < 0)
                    animalIndex = GetAnimalInHoleIndex(symbol);

                if (animalIndex >= 0)
                    result = new MovableAnimal(this, new Point(x, y), animalIndex);
            }

        }
        public void SelectAnimal(MovableAnimal animal)
        {
            selector.SelectedAnimal = animal;
        }
        int GetAnimalIndex(char symbol)
        {
            return MovableAnimalLetters.IndexOf(symbol);
        }
        int GetAnimalInHoleIndex(char symbol)
        {
            return MovableAnimalLetters.ToUpper().IndexOf(symbol);
        }

        private void AddTile(int x, int y, char symbol)
        {
                Tile tile = new Tile(CharToTileType(symbol), x, y);
                tile.LocalPosition = GetCellPosition(x, y);
                tiles[x, y] = tile;
        }
        Tile.Type CharToTileType(char symbol)
        {
            switch (symbol)
            {
                // standard cases
                case ' ': return Tile.Type.Empty;
                case '.': return Tile.Type.Normal;
                case '#': return Tile.Type.Wall;
                case '_': return Tile.Type.Hole;
                // every other symbol can be either a hole or a normal tile
                default:
                    if (GetAnimalInHoleIndex(symbol) > 0)
                        return Tile.Type.Hole;
                    return Tile.Type.Normal;
            }
        }

        void AddLevelInfoObjects(string title, string description)
        {
            // - background box
            SpriteGameObject infoBackground = new SpriteGameObject("Sprites/spr_level_info");
            infoBackground.SetOriginToCenter();
            infoBackground.LocalPosition = new Vector2(600, 820);
            AddChild(infoBackground);

            // - title text
            TextGameObject titleText = new TextGameObject("Fonts/HelpFont", Color.Blue, TextGameObject.Alignment.Center);
            titleText.Text = LevelIndex + " - " + title;
            titleText.LocalPosition = new Vector2(600, 786);
            AddChild(titleText);

            // - description text
            TextGameObject descriptionText = new TextGameObject("Fonts/HelpFont", Color.DarkBlue, TextGameObject.Alignment.Center);
            descriptionText.Text = description;
            descriptionText.LocalPosition = new Vector2(600, 820);
            AddChild(descriptionText);

            pairList = new PairList(targetNumberOfPairs);
            pairList.LocalPosition = new Vector2(20, 20);
            AddChild(pairList);
        }

        private int StringToDirection(string direction)
        {
            if(direction == "right")
            {
                return 0;
            }
            else if(direction == "up")
            {
                return 1;
            }   
            else if(direction == "left")
            {
                return 2;
            }
            return 3; //direction == down
        }
        public void AddAnimalToGrid(Animal animal, Point gridPosition)
        {
            animalsOnTiles[gridPosition.X, gridPosition.Y] = animal;
        }
        public void RemoveAnimalFromGrid(Point gridPosition)
        {
            FirstMoveMade = true;
            animalsOnTiles[gridPosition.X, gridPosition.Y] = null;
        }
        public void PairFound(MovableAnimal penguin1, MovableAnimal penguin2)
        {
            int penguinType = MathHelper.Max(penguin1.AnimalIndex, penguin2.AnimalIndex);
            pairList.AddPair(penguinType);

            if (pairList.Completed)
            {
                PlayingState playingState = (PlayingState)ExtendedGame
                .GameStateManager.GetGameState(PenguinPairs.StateName_Playing);
                playingState.LevelCompleted(LevelIndex);
            }
        }
        public override void Reset()
        {
            for (int y = 0; y < GridHeight; y++)
                for (int x = 0; x < GridWidth; x++)
                    animalsOnTiles[x, y] = null;
            FirstMoveMade = false;
            base.Reset();
        }
    }
}
