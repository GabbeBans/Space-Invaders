using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ACTUAL_monogame__Dragons_Clash_COC_inspired_super_cool_game
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D myShip, enemy, bullet;
        List<Vector2> enemyList, bulletList;
        Vector2 enemySpeed, bulletSpeed;
        Vector2 myPosition;
        Vector2 mySpeed;
        Rectangle myHitBox;
        SpriteFont myFont;
        int enemySpawnRate;
        int enemyTimer;
        int enemyScale = 1;
        Random rNG;
        bool IsGameOver = false; // Declare IsGameOver as a member variable

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            mySpeed = new Vector2(0, 0);
            enemyList = new List<Vector2>();
            enemySpeed = new Vector2(0, 2);
            enemySpawnRate = 100;
            rNG = new Random();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            myShip = Content.Load<Texture2D>("spaceship");
            enemy = Content.Load<Texture2D>("enemy");
            myFont = Content.Load<SpriteFont>("GameOver");

            // Ensure the player stays within the game window bounds
            myPosition.X = MathHelper.Clamp(myPosition.X, 0, Window.ClientBounds.Width - myShip.Width);
            myPosition.Y = MathHelper.Clamp(myPosition.Y, 0, Window.ClientBounds.Height - myShip.Height);

            myHitBox = new Rectangle((int)myPosition.X, (int)myPosition.Y, 40, 40);

            // Set the preferred window size to fullscreen
            _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            _graphics.ApplyChanges();
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            var state = Keyboard.GetState();

            // Reset the player's speed on each frame
            mySpeed = Vector2.Zero;

        if(!IsGameOver) {
            // Adjust the player's speed based on the keyboard input
            if (state.IsKeyDown(Keys.W))
            {
                mySpeed.Y -= 5f; // Move upwards (negative Y direction)
            }

            if (state.IsKeyDown(Keys.S))
            {
                mySpeed.Y += 5f; // Move downwards (positive Y direction)
            }

            if (state.IsKeyDown(Keys.A))
            {
                mySpeed.X -= 5f; // Move left (negative X direction)
            }

            if (state.IsKeyDown(Keys.D))
            {
                mySpeed.X += 5f; // Move right (positive X direction)
            }
        }

            // Adjust the player's position based on the speed
            myPosition += mySpeed;

            // Update the player's hitbox position
            myHitBox.X = (int)myPosition.X;
            myHitBox.Y = (int)myPosition.Y;

            // Increment the enemyTimer
            enemyTimer++;

            // Check if it's time to spawn a new enemy
            if (enemyTimer >= enemySpawnRate)
            {
                int enemyPosX = rNG.Next(0, Window.ClientBounds.Width - (int)(enemy.Width * enemyScale));
                enemyList.Add(new Vector2(enemyPosX, -enemy.Height));
                enemyTimer = 0; // Reset the enemyTimer
            }

            // Move existing enemies and check for collisions
            if (!IsGameOver)
            {
                for (int i = 0; i < enemyList.Count; i++)
                {   
                    enemyList[i] += enemySpeed;
                    // Update the enemy's hitbox position and size
                    // Check for collisions
                    Rectangle enemyHitBox = new Rectangle((int)enemyList[i].X, (int)enemyList[i].Y, 40, 40);
                    if (enemyHitBox.Intersects(myHitBox))
                    {
                        IsGameOver = true;
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            // Draw the spaceship
            _spriteBatch.Draw(myShip, myPosition, null, Color.White, 0f, Vector2.Zero, enemyScale, SpriteEffects.None, 0f);

            // Draw the enemy
            for (int i = 0; i < enemyList.Count; i++)
            {
                _spriteBatch.Draw(enemy, enemyList[i], null, Color.White, 0f, Vector2.Zero, enemyScale, SpriteEffects.None, 0f);
            }

            // Draw "Game Over" text if game is over
            if (IsGameOver)
            {
                // Calculate the position to center the text
                Vector2 gameOverPosition = new Vector2(
                    (GraphicsDevice.Viewport.Width - myFont.MeasureString("Game Over").X) / 2,
                    (GraphicsDevice.Viewport.Height - myFont.MeasureString("Game Over").Y) / 2);

                // Draw the "Game Over" text
                _spriteBatch.DrawString(myFont, "Game Over", gameOverPosition, Color.Red);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
