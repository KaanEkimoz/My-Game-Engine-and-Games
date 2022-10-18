using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace TickTick.LevelObjects
{
    class Player : AnimatedGameObject
    {
        const float walkingSpeed = 400; // Standard walking speed, in pixels per second.
        bool facingLeft; // Whether or not the character is currently looking to the left.
        const float jumpSpeed = 900; // lift−off speed when the character jumps
        const float gravity = 2300; // strength of the gravity force
        const float maxFallSpeed = 1200; // maximum vertical speed when the character is falling
        bool isGrounded;
        bool standingOnIceTile, standingOnHotTile;
        float desiredHorizontalSpeed;

        const float iceFriction = 1;
        const float normalFriction = 20;
        const float airFriction = 5;

        protected Vector2 startPosition;

        bool isCelebrating = false;

        Level level;

        public bool CanCollideWithObjects { get { return (IsAlive && !isCelebrating); } }
        public bool IsFalling
        {
            get { return velocity.Y > 0 && !isGrounded; }
        }
        public bool IsMoving { get { return velocity != Vector2.Zero; } }

        public bool IsAlive { get; private set; }

        private bool isExploding;

        Rectangle BoundingBoxForCollisions
        {
            get
            {
                Rectangle bbox = BoundingBox;
                bbox.X += 20;
                bbox.Width -= 40;
                bbox.Height += 1;
                return bbox;
            }
        }

        public Player(Level level, Vector2 startPosition) : base(TickTick.Depth_LevelPlayer)
        {
            // load all animations
            LoadAnimation("Sprites/LevelObjects/Player/spr_idle", "idle", true, 0.1f);
            LoadAnimation("Sprites/LevelObjects/Player/spr_run@13", "run", true, 0.04f);
            LoadAnimation("Sprites/LevelObjects/Player/spr_jump@14", "jump", false, 0.08f);
            LoadAnimation("Sprites/LevelObjects/Player/spr_celebrate@14", "celebrate", false, 0.05f);
            LoadAnimation("Sprites/LevelObjects/Player/spr_die@5", "die", true, 0.1f);
            LoadAnimation("Sprites/LevelObjects/Player/spr_explode@5x5", "explode", false, 0.04f);

            this.startPosition = startPosition;
            this.level = level;

            Reset();
        }
        public override void Update(GameTime gameTime)
        {
            Vector2 previousPosition = localPosition;

            if(!isCelebrating && CanCollideWithObjects)
            {
                float friction;
                if (standingOnIceTile)
                    friction = iceFriction;
                else if (isGrounded)
                    friction = normalFriction;
                else
                    friction = airFriction;
                float multiplier = MathHelper.Clamp(friction * (float)gameTime.ElapsedGameTime.TotalSeconds, 0, 1);

                velocity.X += (desiredHorizontalSpeed - velocity.X) * multiplier;
                if (Math.Abs(velocity.X) < 1)
                    velocity.X = 0;
            }
            else
            {
                velocity.X = 0;
            }
            ApplyGravity(gameTime);
            base.Update(gameTime);
            if(IsAlive)
            {
                HandleTileCollisions(previousPosition);
                if (standingOnHotTile)
                    level.Timer.Multiplier = 2.0f;
                else
                    level.Timer.Multiplier = 1.0f;
            }
            if (BoundingBox.Center.Y > level.BoundingBox.Bottom)
                Die();
        }
        public override void HandleInput(InputHelper inputHelper)
        {
            if (isCelebrating)
                return;
            if (!CanCollideWithObjects)
                return;

            // arrow keys: move left or right
            if (inputHelper.KeyDown(Keys.Left))
            {
                facingLeft = true;
                desiredHorizontalSpeed = -walkingSpeed;
                if(isGrounded)
                    PlayAnimation("run");
            }
            else if (inputHelper.KeyDown(Keys.Right))
            {
                facingLeft = false;
                desiredHorizontalSpeed = walkingSpeed;
                if(isGrounded)
                    PlayAnimation("run");
            }
            // no arrow keys: don't move
            else
            {
                desiredHorizontalSpeed = 0;
                if(isGrounded)
                    PlayAnimation("idle");
            }
            if (isGrounded && inputHelper.KeyPressed(Keys.Space))
            {
                Jump();
            }
            if (!isGrounded)
                PlayAnimation("jump", false, 8);

            // set the origin to the character's feet
            SetOriginToBottomCenter();

            // make sure the sprite is facing the correct direction
            sprite.Mirror = facingLeft;
        }
        public override void Reset()
        {
            // go back to the starting position
            localPosition = startPosition;
            velocity = Vector2.Zero;
            desiredHorizontalSpeed = 0;
            // start with the idle sprite
            PlayAnimation("idle", true);
            SetOriginToBottomCenter();
            facingLeft = false;
            isGrounded = true;
            standingOnIceTile = standingOnHotTile = false;
            IsAlive = true;
            isCelebrating = false;
            isExploding = false;
        }
        public void Jump(float speed = jumpSpeed)
        {
            velocity.Y = -speed;
            // play the jump animation; always make sure that the animation restarts
            PlayAnimation("jump", true);
            ExtendedGame.AssetManager.PlaySoundEffect("snd_player_jump");
        }
        public void Celebrate()
        {
            isCelebrating = true;
            PlayAnimation("celebrate");
            SetOriginToBottomCenter();
            // stop moving
            velocity = Vector2.Zero;
        }
        public void Explode()
        {
            IsAlive = false;
            isExploding = true;
            PlayAnimation("explode");
            velocity = Vector2.Zero;
            ExtendedGame.AssetManager.PlaySoundEffect("snd_player_explode");
        }
        void HandleTileCollisions(Vector2 previousPosition)
        {
            isGrounded = false;
            standingOnIceTile = false;
            standingOnHotTile = false;

            Rectangle bbox = BoundingBoxForCollisions;
            Point topLeftTile = level.GetTileCoordinates(new Vector2(bbox.Left, bbox.Top)) - new Point(1, 1);
            Point bottomRightTile = level.GetTileCoordinates(new Vector2(bbox.Right, bbox.Bottom)) + new Point(1, 1);
            for(int y = topLeftTile.Y; y <= bottomRightTile.Y; y++)
{
                for (int x = topLeftTile.X; x <= bottomRightTile.X; x++)
                {
                    Tile.Type tileType = level.GetTileType(x, y);
                    if (tileType == Tile.Type.Empty)
                        continue;
                    Vector2 tilePosition = level.GetCellPosition(x, y);
                    if (tileType == Tile.Type.Platform
                    && localPosition.Y > tilePosition.Y && previousPosition.Y > tilePosition.Y)
                        continue;
                    Rectangle tileBounds = new Rectangle((int)tilePosition.X, (int)tilePosition.Y, Level.TileWidth, Level.TileHeight);
                    if (!tileBounds.Intersects(bbox))
                        continue;
                    Rectangle overlap = CollisionDetection.CalculateIntersection(bbox, tileBounds);
                    if (overlap.Width < overlap.Height)
                    {
                        // handle a horizontal collision
                        if ((velocity.X >= 0 && bbox.Center.X < tileBounds.Left) || // right wall
                        (velocity.X <= 0 && bbox.Center.X > tileBounds.Right)) // left wall
                        {
                            localPosition.X = previousPosition.X;
                            velocity.X = 0;
                        }
                    }
                    else
                    {
                        // handle a vertical collision
                        if (velocity.Y >= 0 && bbox.Center.Y < tileBounds.Top && overlap.Width > 6) //floor
                        {
                            isGrounded = true;
                            Tile.SurfaceType surface = level.GetSurfaceType(x, y);
                            if (surface == Tile.SurfaceType.Hot)
                                standingOnHotTile = true;
                            else if (surface == Tile.SurfaceType.Ice)
                                standingOnIceTile = true;
                            velocity.Y = 0;
                            localPosition.Y = tileBounds.Top;
                        }
                        else if (velocity.Y <= 0 && bbox.Center.Y > tileBounds.Bottom && overlap.Height > 2) //ceiling
                        {
                            localPosition.Y = previousPosition.Y;
                            velocity.Y = 0;
                        }
                    }
                }
            }
        }
        void SetOriginToBottomCenter()
        {
            Origin = new Vector2(sprite.Width / 2, sprite.Height);
        }
        void ApplyGravity(GameTime gameTime)
        {
            velocity.Y += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (velocity.Y > maxFallSpeed)
                velocity.Y = maxFallSpeed;
        }
        public void Die()
        {
            level.Timer.Running = false;
            IsAlive = false;
            PlayAnimation("die");
            velocity = new Vector2(0, -jumpSpeed);
            ExtendedGame.AssetManager.PlaySoundEffect("snd_player_die");
        }
    }
}
