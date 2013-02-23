#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System.Collections;
using System.IO;

using LetsMakeAGame.Players;
#endregion

namespace LetsMakeAGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        public static ContentManager contentMgr;
        SpriteBatch spriteBatch;
        Texture2D playerTexture;

        SpriteFont font;
        public static Player player;
        public static Vector2 center;

        ////////////////////////////////////////TEST
        public static Texture2D cloud;
        public static Texture2D engineeringBlock;
        ////////////////////////////////////////TEST

        public static GraphicsDevice gd;

        public static Level level;
        public static LevelEditor le;

        public static Hashtable textureLookupTable;

        int tsaX;
        int tsaY;
        const int PLAYER_MOVE_SPEED = 6;
        int ground;

        public static Viewport viewport;

        public static float scale;

        public static KeyboardState currentKeyboardState;
        public static KeyboardState previousKeyboardState;

        public static MouseState currentMouseState;
        public static MouseState previousMouseState;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            gd = this.GraphicsDevice;
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = false;
            //ResolutionChooser r = new ResolutionChooser();
            //r.Show();
            viewport = this.GraphicsDevice.Viewport;
            int width = 1600;
            int height = 900;
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            viewport.Width = graphics.PreferredBackBufferWidth;
            viewport.Height = graphics.PreferredBackBufferHeight;
            scale = (float)((double)(width * height) / (double)(1600 * 900));
            center = new Vector2((int)graphics.PreferredBackBufferWidth / 2, (int)graphics.PreferredBackBufferHeight / 2);
            contentMgr = this.Content;
            textureLookupTable = new Hashtable();
            StreamReader sr = new StreamReader("Content/textureRep.txt");
            string line = sr.ReadLine();
            while (line != null)
            {
                string key = line.Substring(0, line.IndexOf(" "));
                string value = line.Substring(line.IndexOf(" ") + 1, line.Length - line.IndexOf(" ") - 1);
                textureLookupTable.Add(key, value);
                line = sr.ReadLine();
            }
            sr.Close();
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
            this.IsMouseVisible = true;
            player = new Artist();
            ground = GraphicsDevice.Viewport.TitleSafeArea.Height - 40;
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
            //DEBUG
            font = Content.Load<SpriteFont>("myFont");
            ///////
            cloud = Content.Load<Texture2D>("Cloud");
            engineeringBlock = Content.Load<Texture2D>("engineeringBlock");


            
            player.Initialize(playerTexture, new Vector2(60, GraphicsDevice.Viewport.TitleSafeArea.Height - playerTexture.Height), GraphicsDevice.Viewport);
            List<string> songNames = new List<string>();
            List<string> sfxNames = new List<string>();
            //level = new Level("background", "foreground", "Content/Maps/impossibleMap.txt", songNames, sfxNames);
            le = new LevelEditor();
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
            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
            // TODO: Add your update logic here
            //level.GetInput(currentKeyboardState, previousKeyboardState, currentMouseState, previousMouseState, gameTime);
            le.GetInput(currentKeyboardState, previousKeyboardState, currentMouseState, previousMouseState, gameTime);
            
            previousKeyboardState = currentKeyboardState;
            previousMouseState = currentMouseState;

            le.Update(gameTime);
            //level.Update(gameTime);
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
            //level.Draw(spriteBatch);
            le.Draw(spriteBatch);
            //DEBUG
            //spriteBatch.DrawString(font, "TSArea X: " + tsaX, new Vector2(0, 0), Color.Gray);
            //spriteBatch.DrawString(font, "TSArea Y: " + tsaY, new Vector2(0, 20), Color.Gray);
            //spriteBatch.DrawString(font, "Player X: " + player.boundary.X, new Vector2(0, 40), Color.Gray);
            //spriteBatch.DrawString(font, "Player Y: " + player.boundary.Y, new Vector2(0, 60), Color.Gray);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public static Rectangle getRect(Vector2 position, Texture2D texture)
        {
            return new Rectangle((int)(position.X * scale), (int)(position.Y * scale), (int)(texture.Width * scale), (int)(texture.Height * scale));
        }

        public static Rectangle getRect(Texture2D texture)
        {
            return new Rectangle(0, 0, (int)(texture.Width * scale), (int)(texture.Height * scale));
        }

        public static Texture2D getTexture(string name)
        {
            return contentMgr.Load<Texture2D>(name);
        }
    }
}
