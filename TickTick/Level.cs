using Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using TickTick.LevelObjects;

namespace TickTick
{
    partial class Level : GameObjectList
    {
        public const int TileWidth = 72;
        public const int TileHeight = 55;
        bool completionDetected = false;

        public BombTimer Timer { get; private set; }

        Tile[,] tiles;
        List<WaterDrop> waterDrops;
        public Player Player { get; private set; }
        public int LevelIndex { get; private set; }

        bool AllDropsCollected
        {
            get
            {
                foreach (var waterDrop in waterDrops)
                {
                    if (waterDrop.Visible)
                        return false;
                }
                return true;
            }
        }

        SpriteGameObject goal;

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle(0, 0,
                    tiles.GetLength(0) * TileWidth,
                    tiles.GetLength(1) * TileHeight);
            }
        }


        public Level(int levelIndex, string filename)
        {
            LevelIndex = levelIndex;
            completionDetected = false;

            // load the background
            GameObjectList backgrounds = new GameObjectList();
            SpriteGameObject backgroundSky = new SpriteGameObject("Sprites/Backgrounds/spr_sky", TickTick.Depth_Background);
            backgroundSky.LocalPosition = new Vector2(0, 825 - backgroundSky.Height);
            backgrounds.AddChild(backgroundSky);
            AddChild(backgrounds);

            // load the rest of the level
            LoadLevelFromFile(filename);

            Timer = new BombTimer();
            Timer.AddChild(this);

            for (int i = 0; i < 4; i++)
            {
                SpriteGameObject mountain = new SpriteGameObject("Sprites/Backgrounds/spr_mountain_"+(ExtendedGame.Random.Next(2) + 1),
                TickTick.Depth_Background + 0.01f * (float)ExtendedGame.Random.NextDouble());

                mountain.LocalPosition = new Vector2(mountain.Width * (i-1) * 0.4f, BoundingBox.Height - mountain.Height);

                backgrounds.AddChild(mountain);
            }
            for (int i = 0; i < 6; i++)
                backgrounds.AddChild(new Cloud(this));
        }
        public override void Update(GameTime gameTime)
        {
            if (!completionDetected && AllDropsCollected && Player.HasPixelPreciseCollision(goal))
            {
                completionDetected = true;
                Timer.Running = false;
                ExtendedGameWithLevels.GetPlayingState().LevelCompleted(LevelIndex);
                Player.Celebrate();
            }
            else if (Player.IsAlive && Timer.HasPassed)
                Player.Explode();
        }
        public override void Reset()
        {
            base.Reset();
            completionDetected = false;
        }
        public Tile.Type GetTileType(int x, int y)
        {
            if (x < 0 || x >= tiles.GetLength(0))
                return Tile.Type.Wall;
            if (y < 0 || y >= tiles.GetLength(1))
                return Tile.Type.Empty;
            return tiles[x, y].TileType;
        }
        public Tile.SurfaceType GetSurfaceType(int x, int y)
        {
            // If the tile with these coordinates doesn't exist, return the normal surface type.
            if (x < 0 || x >= tiles.GetLength(0) || y < 0 || y >= tiles.GetLength(1))
                return Tile.SurfaceType.Normal;

            // Otherwise, return the actual surface type of the tile.
            return tiles[x, y].Surface;
        }
        public Vector2 GetCellPosition(int x, int y)
        {
            return new Vector2(x * TileWidth, y * TileHeight);
        }
        public Point GetTileCoordinates(Vector2 position)
        {
            return new Point((int)Math.Floor(position.X / TileWidth),
            (int)Math.Floor(position.Y / TileHeight));
        }
    }
}


