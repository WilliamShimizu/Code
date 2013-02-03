#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace LetsMakeAGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D playerTexture;
        Texture2D bg1;
        Texture2D bg2;

        SpriteFont font;
        public static Player player;
        public static Vector2 center;

        public static Texture2D leopard;
        public static Texture2D stone;
        public static Texture2D stars;

        public static GraphicsDevice gd;

        public static Level level;

        int tsaX;
        int tsaY;
        const int PLAYER_MOVE_SPEED = 6;
        int ground;

        public static float scale;

        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            gd = this.GraphicsDevice;
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = false;
            //ResolutionChooser r = new ResolutionChooser();
            //r.Show();
            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;
            center = new Vector2((int)graphics.PreferredBackBufferWidth / 2, (int)graphics.PreferredBackBufferHeight / 2);
            //List<string> MBOPTIONS = new List<string>();
            //MBOPTIONS.Add("OK");
            //string msg = "Text that was typed on the keyboard will be displayed here.\nClick OK to continue...";
            //IAsyncResult result = Guide.BeginShowMessageBox(
            //        "hello", msg, MBOPTIONS, 0,
            //        MessageBoxIcon.Alert, null, null);
            
            //Guide.EndShowMessageBox(result);
            graphics.ToggleFullScreen();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            player = new Player();
            ground = GraphicsDevice.Viewport.TitleSafeArea.Height - 40;
            scale = (float)((double)(1600 * 900) / (double)(1600 * 900));
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            playerTexture = Content.Load<Texture2D>("Block");
            bg1 = Content.Load<Texture2D>("background");
            bg2 = Content.Load<Texture2D>("foreground");
            //DEBUG
            font = Content.Load<SpriteFont>("myFont");
            stars = Content.Load<Texture2D>("starsTile");
            leopard = Content.Load<Texture2D>("leopardTile");
            stone = Content.Load<Texture2D>("rockTile");
            
            player.Initialize(playerTexture, new Vector2(60, GraphicsDevice.Viewport.TitleSafeArea.Height - playerTexture.Height), GraphicsDevice.Viewport);
            level = new Level(bg1, bg2);
            
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
            // TODO: Add your update logic here
            GetInput();
            level.Update(gameTime);
            //DEBUG
            tsaX = graphics.GraphicsDevice.Viewport.TitleSafeArea.Width;
            tsaY = graphics.GraphicsDevice.Viewport.TitleSafeArea.Height;
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            level.Draw(spriteBatch);
            //DEBUG
            spriteBatch.DrawString(font, "TSArea X: " + tsaX, new Vector2(0, 0), Color.Gray);
            spriteBatch.DrawString(font, "TSArea Y: " + tsaY, new Vector2(0, 20), Color.Gray);
            spriteBatch.DrawString(font, "Player X: " + player.boundary.X, new Vector2(0, 40), Color.Gray);
            spriteBatch.DrawString(font, "Player Y: " + player.boundary.Y, new Vector2(0, 60), Color.Gray);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        private void GetInput()
        {
            if (currentKeyboardState.IsKeyDown(Keys.A))
            {
                player.speedX = -PLAYER_MOVE_SPEED;
            }
            else if (currentKeyboardState.IsKeyDown(Keys.D))
            {
                player.speedX = PLAYER_MOVE_SPEED;
            }
            else player.speedX = 0;
            if (currentKeyboardState.IsKeyDown(Keys.W))
            {
                player.speedY = -PLAYER_MOVE_SPEED;
            }
            else if (currentKeyboardState.IsKeyDown(Keys.S))
            {
                player.speedY = PLAYER_MOVE_SPEED;
            }
            else if(!player.jumped) player.speedY = 0;
            if (currentKeyboardState.IsKeyDown(Keys.Space) && !player.jumped)
            {
                player.Jump(PLAYER_MOVE_SPEED);
            }
        }
    }
}
