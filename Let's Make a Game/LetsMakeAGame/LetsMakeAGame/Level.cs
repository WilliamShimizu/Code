using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

using LetsMakeAGame.Players;

namespace LetsMakeAGame
{
    public class Level
    {
        public Background background;
        public Background foreground;
        public List<Tile> tiles;
        List<string> lines;
        Player player;
        public List<Texture2D> textures;
        HashSet<string> textureNames;
        List<string> songNames;
        List<string> soundEffectNames;
        public List<EngineeringBlock> blocks;
        public List<Dot> dots;
        const int PLAYER_MOVE_SPEED = 6;
        public List<Player> players;

        /// <summary>
        /// Constructs the major things needed for the level
        /// </summary>
        /// <param name="backgroundName">Name of the background Image</param>
        /// <param name="foregroundName">Name of the foreground Image</param>
        /// <param name="mapPath">Name of the Map</param>
        /// <param name="songNames">List of song names that will be played in the level</param>
        /// <param name="soundEffects"></param>
        public Level(string backgroundName, string foregroundName, string mapPath, List<string> songNames, List<string> soundEffects)
        {
            this.textures = new List<Texture2D>();
            this.textureNames = new HashSet<string>();
            this.songNames = songNames;
            this.soundEffectNames = soundEffects;
            StreamReader sr = new StreamReader(mapPath);
            string line = sr.ReadLine();
            string[] names = line.Split(' ');
            foreach (string textureName in names)
            {
                if (textureName == "!") continue;
                this.textureNames.Add(textureName);
            }
            LoadContent(backgroundName, foregroundName);
            player = Game1.player;
            if (player is Engineer)
            {
                blocks = ((Engineer)player).blocks;
            }
            if (player is Artist)
            {
                dots = ((Artist)player).dots;
            }
            tiles = new List<Tile>();
            lines = new List<string>();
            while (line != null)
            {
                lines.Add(line);
                line = sr.ReadLine();
            }
            sr.Close();
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].Contains("!")) continue;
                for (int j = 0; j < lines[i].Length; j++)
                {
                    string s = lines[i].Substring(j, 1);
                    if (s == " " || s == "\n") continue;
                    Tile t = new Tile(this, s, new Vector2(j, i));
                    tiles.Add(t);
                }
            }
        }

        /// <summary>
        /// Load Content such as textures, songs, sfx, etc.
        /// </summary>
        /// <param name="bg">Name of the background file</param>
        /// <param name="fg">Name of the foreground file</param>
        public void LoadContent(string bg, string fg)
        {
            if (bg != "null")
            {
                Texture2D bg1 = Game1.contentMgr.Load<Texture2D>(bg);
                this.background = new Background(bg1);
            }
            if (fg != "null")
            {
                Texture2D fg1 = Game1.contentMgr.Load<Texture2D>(fg);
                this.foreground = new Background(fg1);
            }
            if (textureNames != null && textureNames.Count > 0)
            {
                foreach (string texture in textureNames) textures.Add(Game1.contentMgr.Load<Texture2D>(texture));
            }
            if (songNames != null && songNames.Count > 0)
            {
                foreach (string song in songNames) Game1.contentMgr.Load<Song>(song);
            }
            if (soundEffectNames != null && soundEffectNames.Count > 0)
            {
                foreach (string sfx in soundEffectNames) Game1.contentMgr.Load<SoundEffect>(sfx);
            }
        }

        /// <summary>
        /// Unload content when the level has been completed.
        /// </summary>
        public void UnloadContent()
        {

        }

        /// <summary>
        /// Updates the position of everything in relation to the player.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            player.Update(gameTime);
            foreach (Tile t in tiles) t.Update(player);
            CheckCollision();
            if(background != null) background.Update(player.boundary, player.speedX / 3, player.speedY / 3);
            if(foreground != null) foreground.Update(player.boundary, player.speedX / 2, player.speedY / 2);
        }

        /// <summary>
        /// Draws the backgrounds, tiles, and player.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch used from the Game1</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if(background != null) background.Draw(spriteBatch);
            if(foreground != null) foreground.Draw(spriteBatch);
            foreach (Tile t in tiles) spriteBatch.Draw(t.texture, t.boundary, null, Color.White);
            player.Draw(spriteBatch);
        }

        public void GetInput(KeyboardState currentKeyboardState, KeyboardState previousKeyboardState, MouseState currentMouseState, MouseState previousMouseState, GameTime gameTime)
        {
            //Mouse
            if (currentMouseState.LeftButton == ButtonState.Pressed)
            {
                if (player is Artist)
                {
                    ((Artist)player).Special(new Vector2(currentMouseState.X, currentMouseState.Y));
                }
                if (player is QA && previousMouseState.LeftButton == ButtonState.Released)
                {
                    ((QA)player).Special(new Vector2(currentMouseState.X, currentMouseState.Y), this);
                }
            }

            if (currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
            {
                if (player is Artist)
                {
                    ((Artist)player).ReleaseSpecial();
                }
            }

            //Keyboard
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
            if (currentKeyboardState.IsKeyDown(Keys.Space) && player.canJump && !previousKeyboardState.IsKeyDown(Keys.Space))
            {
                player.Jump(PLAYER_MOVE_SPEED);
            }
            if (currentKeyboardState.IsKeyDown(Keys.LeftShift) && !previousKeyboardState.IsKeyDown(Keys.LeftShift)) player.Special();
            if (player is Musician)
            {
                if (currentKeyboardState.IsKeyUp(Keys.LeftShift) && previousKeyboardState.IsKeyDown(Keys.LeftShift)) ((Musician)player).ReleaseSpecial();
            }
            //DEBUG
            if (currentKeyboardState.IsKeyDown(Keys.G))
            {
                if (player.gravityIsOn)
                {
                    player.gravityIsOn = false;
                    player.speedY = 0;
                }
                else player.gravityIsOn = true;
            }
            ////////
            
        }

        /// <summary>
        /// Updates collision for player and engineering blocks
        /// </summary>
        public void CheckCollision()
        {
            if (dots != null && dots.Count > 0)
            {
                player.UpdateCollisionBoundaries(false);
                foreach (Dot dot in dots)
                {
                    if (player.boundary.Intersects(dot.boundary))
                    {
                        if (player.top.Intersects(dot.boundary)) continue;
                        BasicCollision(player, dot.boundary);
                    }
                }
            }
            player.UpdateCollisionBoundaries(true);
            foreach (Tile t in tiles)
            {
                //If the player lands on the right or left hand side of the upper corner, do this, otherwise, they could get stuck.
                if (player.boundary.Intersects(t.boundary))
                {
                    BasicCollision(player, t.boundary);
                }
                if (blocks != null && blocks.Count > 0)
                {
                    foreach (EngineeringBlock block in blocks)
                    {
                        /////////////////////////////////This is no good.
                        if (block.boundary.Intersects(t.boundary))
                        {
                            if (block.boundary.Y + block.boundary.Height >= t.boundary.Y) block.boundary.Y = t.boundary.Y - block.boundary.Height;
                            if (block.boundary.Y <= t.boundary.Y + t.boundary.Height) block.boundary.Y = t.boundary.Y + t.boundary.Height;
                            if (block.boundary.X + block.boundary.Width >= t.boundary.X) block.boundary.X = t.boundary.X + block.boundary.Width;
                            if (block.boundary.X <= t.boundary.X + t.boundary.Width) block.boundary.X = t.boundary.X + t.boundary.Width;
                        }
                    }
                }
            }
        }

        private void BasicCollision(Player player, Rectangle other)
        {
            if (player.bottom.Intersects(other) && (player.left.Intersects(other) || player.right.Intersects(other)))
            {
                player.boundary.Y = other.Top - player.boundary.Height;
                player.speedY = 0;
                player.canJump = true;
                return;
            }
            if (player.bottom.Intersects(other))
            {
                player.boundary.Y = other.Top - player.boundary.Height;
                player.speedY = 0;
                player.canJump = true;
            }
            if (player.top.Intersects(other))
            {
                player.boundary.Y = other.Bottom;
                player.speedY = 6;
            }
            if (player.left.Intersects(other))
            {
                player.boundary.X = other.Right;
                player.speedX = 0;
            }
            if (player.right.Intersects(other))
            {
                player.boundary.X = other.Left - player.boundary.Width;
                player.speedX = 0;
            }
        }
    }
}
