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
using LevelManager;
using LetsMakeAGame.Players;
using Common;
using eContentManager;

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

        public static SpriteFont font;
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

        Camera camera;
        LevelManager.Level lvl;
        eContentManager.eContentManager contentManager;
        Avatar.Player plyr;

        InputHandler.InputManager inputManager;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            gd = this.GraphicsDevice;
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = false;
            viewport = this.GraphicsDevice.Viewport;
            int width = 1600;
            int height = 900;
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            viewport.Width = graphics.PreferredBackBufferWidth;
            viewport.Height = graphics.PreferredBackBufferHeight;
            scale = (float)((double)(width * height) / (double)(1600 * 900));
            center = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
            contentMgr = this.Content;
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
            this.IsMouseVisible = true;
            ground = GraphicsDevice.Viewport.TitleSafeArea.Height - 40;
            camera = new Camera(viewport);
            contentManager = new eContentManager.eContentManager(this.Content);
            lvl = new LevelManager.Level(contentManager, "serializeTest.xml");
            inputManager = new InputHandler.InputManager();
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
            playerTexture = Content.Load<Texture2D>("Tiles/Block");
            //DEBUG
            font = Content.Load<SpriteFont>("myFont");
            
            ///////
            cloud = Content.Load<Texture2D>("Cloud");
            engineeringBlock = Content.Load<Texture2D>("Tiles/engineeringBlock");
            List<string> songNames = new List<string>();
            List<string> sfxNames = new List<string>();
            //le = new LevelEditor();
            

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
            base.Update(gameTime);
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            //le.Update(gameTime);

            plyr = lvl.getActivePlayer();
            plyr.velocity.X = 0f;
            HashSet<InputHandler.InputManager.ACTIONS> actions = inputManager.GetInput();

            float moveSpeed = 5f;
            float friction = 1.5f;
            if (plyr.canJump) moveSpeed -= friction;
            foreach (InputHandler.InputManager.ACTIONS action in actions)
            {
                switch (action)
                {
                    case InputHandler.InputManager.ACTIONS.LEFT:
                        plyr.velocity.X = -moveSpeed;
                        break;
                    case InputHandler.InputManager.ACTIONS.RIGHT:
                        plyr.velocity.X = moveSpeed;
                        break;
                    case InputHandler.InputManager.ACTIONS.UP:
                        plyr.velocity.Y = -moveSpeed;
                        break;
                    case InputHandler.InputManager.ACTIONS.DOWN:
                        plyr.velocity.Y = moveSpeed;
                        break;
                    case InputHandler.InputManager.ACTIONS.JUMP:
                        if(plyr.canJump) plyr.Jump();
                        break;
                    case InputHandler.InputManager.ACTIONS.SPECIAL:
                        if (plyr is Avatar.Players.Designer)
                        {
                            ((Avatar.Players.Designer)plyr).Special();
                        }
                        break;
                    case InputHandler.InputManager.ACTIONS.TOGGLE:
                        lvl.switchPlayers();
                        break;
                    case InputHandler.InputManager.ACTIONS.LEFT_CLICK_DOWN:
                        if (plyr is Avatar.Players.Artist)
                        {
                            ((Avatar.Players.Artist)plyr).Special(camera.getWorldCoord(inputManager.cursorPosition));
                        }
                        break;
                    case InputHandler.InputManager.ACTIONS.RIGHT_CLICK_DOWN:
                        break;
                    case InputHandler.InputManager.ACTIONS.LEFT_CLICK:
                        if (plyr is Avatar.Players.Engineer)
                        {
                            ((Avatar.Players.Engineer)plyr).Special();
                        }
                        else if (plyr is Avatar.Players.Artist)
                        {
                            ((Avatar.Players.Artist)plyr).ReleaseSpecial();
                        }
                        break;
                    case InputHandler.InputManager.ACTIONS.RIGHT_CLICK:
                        if (plyr is Avatar.Players.QA)
                        {
                            ((Avatar.Players.QA)plyr).Special(camera.getWorldCoord(inputManager.cursorPosition), lvl.tiles);
                        }
                        break;
                    default:
                        break;
                }
            }

            lvl.Update(gameTime);

            tsaX = graphics.GraphicsDevice.Viewport.TitleSafeArea.Width;
            tsaY = graphics.GraphicsDevice.Viewport.TitleSafeArea.Height;

            camera.Update(gameTime, plyr.boundary);
            
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            //spriteBatch.Begin();
            //le.Draw(spriteBatch);
            base.Draw(gameTime);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.transform);
            lvl.Draw(spriteBatch);
            spriteBatch.End();

            //base.Draw(gameTime);
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
