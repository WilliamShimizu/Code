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
using System.Xml;

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
        int activePlayerNumber;
        Tile rep;

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
            loadXML();
            player = players[0];
            activePlayerNumber = 0;
            player.isActive = true;
            foreach (Player p in players)
            {
                if (p is Artist) dots = ((Artist)p).dots;
                if (p is Engineer) blocks = ((Engineer)p).blocks;
            }
        }

        //    loadTextFile(mapPath, backgroundName, foregroundName);
        //}

        //private void loadTextFile(string mapPath, string backgroundName, string foregroundName)
        //{
        //    StreamReader sr = new StreamReader(mapPath);
        //    string line = sr.ReadLine();
        //    string[] names = line.Split(' ');
        //    foreach (string textureName in names)
        //    {
        //        if (textureName == "!") continue;
        //        this.textureNames.Add(textureName);
        //    }
        //    LoadContent(backgroundName, foregroundName);
        //    player = Game1.player;

        //    if (player is Engineer)
        //    {
        //        blocks = ((Engineer)player).blocks;
        //    }
        //    if (player is Artist)
        //    {
        //        dots = ((Artist)player).dots;
        //    }
        //    tiles = new List<Tile>();
        //    lines = new List<string>();
        //    while (line != null)
        //    {
        //        lines.Add(line);
        //        line = sr.ReadLine();
        //    }
        //    sr.Close();
        //    for (int i = 0; i < lines.Count; i++)
        //    {
        //        if (lines[i].Contains("!")) continue;
        //        for (int j = 0; j < lines[i].Length; j++)
        //        {
        //            string s = lines[i].Substring(j, 1);
        //            if (s == " " || s == "\n") continue;
        //            Tile t = new Tile(textures, s, new Vector2(j * 40, i * 40));
        //            tiles.Add(t);
        //        }
        //    }
        //}

        private void loadXML()
        {
            tiles = new List<Tile>();
            players = new List<Player>();
            XmlReader reader = XmlReader.Create("serializeTest.xml");
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "Players":
                            reader.Read();
                            loadPlayers(reader, players);
                            break;
                        case "Tiles":
                            reader.ReadStartElement("Tiles");
                            loadTiles(reader, tiles);
                            break;
                        default:
                            break;
                    }
                }
            }
            reader.Close();
        }

        private void loadTiles(XmlReader reader, List<Tile> tiles)
        {
            string name = "";
            Rectangle boundary = new Rectangle();
            while (reader.IsStartElement())
            {
                reader.ReadStartElement("Tile");
                boundary.X = readInt(reader, "positionX");
                boundary.Y = readInt(reader, "positionY");
                boundary.Height = readInt(reader, "height");
                boundary.Width = readInt(reader, "width");
                name = readString(reader, "textureName");
                reader.ReadEndElement();
                tiles.Add(new Tile(Game1.getTexture(name), new Vector2(boundary.X, boundary.Y)));
            }
        }

        private void loadPlayers(XmlReader reader, List<Player> p)
        {
            string name = "";
            Rectangle boundary = new Rectangle();
            while (reader.Value != "Tiles")
            {
                string type = "";
                if (reader.IsStartElement())
                {
                    type = reader.Name.ToString();
                    reader.ReadStartElement();
                }
                else return;
                //try { reader.ReadStartElement("Player"); }
                //catch
                //{
                //    return;
                //}
                boundary.X = readInt(reader, "positionX");
                boundary.Y = readInt(reader, "positionY");
                boundary.Height = readInt(reader, "height");
                boundary.Width = readInt(reader, "width");
                name = readString(reader, "textureName");
                reader.ReadEndElement();
                Player player;
                Texture2D texture = Game1.getTexture(name);
                switch (type)
                {
                    case "Engineer":
                        player = new Engineer(boundary, texture);
                        break;
                    case "Designer":
                        player = new Designer(boundary, texture);
                        break;
                    case "Artist":
                        player = new Artist(boundary, texture);
                        break;
                    case "Musician":
                        player = new Musician(boundary, texture);
                        break;
                    case "QA":
                        player = new QA(boundary, texture);
                        break;
                    default:
                        player = new Player(boundary, texture, Common.Entity.ENTITY.Player);
                        break;
                }
                p.Add(player);
            }
        }

        public static string readString(XmlReader reader, string nodeName)
        {
            reader.ReadStartElement(nodeName);
            string s = reader.ReadContentAsString();
            reader.ReadEndElement();
            return s;
        }

        public static int readInt(XmlReader reader, string nodeName)
        {
            reader.ReadStartElement(nodeName);
            int i = reader.ReadContentAsInt();
            reader.ReadEndElement();
            return i;
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
                foreach (string texture in textureNames) textures.Add(Game1.contentMgr.Load<Texture2D>("Tiles/" + texture));
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
            //player.Update(gameTime);
            string name = "";
            foreach (Player p in players)
            {
                if (p.isActive)
                {
                    p.Update(gameTime);
                    name = p.textureName;
                }
                else if (p is Designer && ((Designer)p).cloud != null) p.Update(gameTime);
                else p.Update(player);
            }
            Tile temp = null;
            foreach (Tile t in tiles)
            {
                if (t.boundary.Width > 40)
                {
                    foreach (Player p in players)
                    {
                        if (p is Designer)
                        {
                            if (((Designer)p).cloud == null) temp = t;
                            else
                            {
                                t.boundary.Y -= 2;
                                if (p.boundary.X >= Game1.center.X + 200 || p.boundary.X <= Game1.center.X - 200) t.boundary.X -= p.speedX;
                                if (((Designer)p).cloud.boundary.Y <= ((Designer)p).cloud.endPointY) ((Designer)p).cloud.isActive = false;
                            }
                        }
                    }
                }
                else t.Update(player);
            }
            foreach (Dot d in dots)
            {
                d.Update(player);
            }
            if(temp != null) tiles.Remove(temp);
            CheckCollision();
            if(background != null) background.Update(player.boundary, player.speedX / 3, player.speedY / 3);
            if(foreground != null) foreground.Update(player.boundary, player.speedX / 2, player.speedY / 2);
            rep = new Tile(Game1.getTexture(name), new Vector2(0, 0));
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
            foreach (Player p in players)
            {
                p.Draw(spriteBatch);
            }
            rep.Draw(spriteBatch);
            //player.Draw(spriteBatch);
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
            if (currentKeyboardState.IsKeyDown(Keys.LeftShift) && !previousKeyboardState.IsKeyDown(Keys.LeftShift))
            {
                if (player is Designer)
                {
                    ((Designer)player).Special();
                    tiles.Add(((Designer)player).cloud);
                }
            }
            if (player is Musician)
            {
                if (currentKeyboardState.IsKeyUp(Keys.LeftShift) && previousKeyboardState.IsKeyDown(Keys.LeftShift)) ((Musician)player).ReleaseSpecial();
            }
            if (previousKeyboardState.IsKeyDown(Keys.T) && currentKeyboardState.IsKeyUp(Keys.T))
            {
                player.isActive = false;
                activePlayerNumber++;
                if (activePlayerNumber > players.Count - 1) activePlayerNumber = 0;
                player = players[activePlayerNumber];
                player.isActive = true;
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
                foreach (Player p in players)
                {
                    p.UpdateCollisionBoundaries(false);
                    foreach (Dot dot in dots)
                    {
                        if (p.boundary.Intersects(dot.boundary))
                        {
                            if (p.top.Intersects(dot.boundary)) continue;
                            BasicCollision(p, dot.boundary);
                        }
                    }
                }
                
            }
            foreach (Player p in players)
            {
                p.UpdateCollisionBoundaries(true);
            }
            foreach (Tile t in tiles)
            {
                //If the player lands on the right or left hand side of the upper corner, do this, otherwise, they could get stuck.
                foreach (Player p in players)
                {
                    if (p.boundary.Intersects(t.boundary))
                    {
                        BasicCollision(p, t.boundary);
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
