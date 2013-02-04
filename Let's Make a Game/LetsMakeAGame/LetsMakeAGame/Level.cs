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

namespace LetsMakeAGame
{
    public class Level
    {
        public Background background;
        public Background foreground;
        public static List<Tile> tiles;
        List<string> lines;
        Player player;
        public List<Texture2D> textures;
        HashSet<string> textureNames;
        List<string> songNames;
        List<string> soundEffectNames;

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
            this.player = Game1.player;
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

        public void UnloadContent()
        {

        }

        public void Update(GameTime gameTime)
        {
            player.Update();
            foreach (Tile t in tiles) t.Update(player);
            CheckCollision();
            if(background != null) background.Update(player.boundary, player.speedX / 3, player.speedY / 3);
            if(foreground != null) foreground.Update(player.boundary, player.speedX / 2, player.speedY / 2);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(background != null) background.Draw(spriteBatch);
            if(foreground != null) foreground.Draw(spriteBatch);
            foreach (Tile t in tiles) spriteBatch.Draw(t.texture, t.boundary, null, Color.White);
            player.Draw(spriteBatch);
        }

        public void CheckCollision()
        {
            foreach (Tile t in tiles)
            {
                if (player.boundary.Intersects(t.boundary))
                {
                    //If the player lands on the right or left hand side of the upper corner, do this, otherwise, they could get stuck.
                    if (player.bottom.Intersects(t.boundary) && (player.left.Intersects(t.boundary) || player.right.Intersects(t.boundary)))
                    {
                        player.boundary.Y = t.boundary.Top - player.boundary.Height;
                        player.jumped = false;
                        player.speedY = 0;
                        player.canJump = true;
                        continue;
                    }
                    if (player.bottom.Intersects(t.boundary))
                    {
                        player.boundary.Y = t.boundary.Top - player.boundary.Height;
                        player.jumped = false;
                        player.speedY = 0;
                        player.canJump = true;
                    }
                    else player.canJump = false;
                    if (player.top.Intersects(t.boundary))
                    {
                        player.boundary.Y = t.boundary.Bottom;
                        player.speedY = 6;
                    }
                    if (player.left.Intersects(t.boundary))
                    {
                        player.boundary.X = t.boundary.Right;
                        player.speedX = 0;
                    }
                    if (player.right.Intersects(t.boundary))
                    {
                        player.boundary.X = t.boundary.Left - player.boundary.Width;
                        player.speedX = 0;
                    }
                }
            }
        }
    }
}
