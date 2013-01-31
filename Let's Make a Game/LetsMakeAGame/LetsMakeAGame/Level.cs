using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public Level(Texture2D background, Texture2D foreground)
        {
            this.background = new Background();
            this.foreground = new Background();
            this.background.Initialize(background);
            this.foreground.Initialize(foreground);
            this.player = Game1.player;
            tiles = new List<Tile>();
            lines = new List<string>();
            StreamReader sr = new StreamReader("Content/Maps/testMap.txt");
            string line = sr.ReadLine();
            while (line != null)
            {
                lines.Add(line);
                line = sr.ReadLine();
            }
            sr.Close();
            for (int i = 0; i < lines.Count; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    String s = lines[i].Substring(j, 1);
                    if (s == " " || s == "\n") continue;
                    Tile t = new Tile(Int16.Parse(s), new Vector2(j, i));
                    tiles.Add(t);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            CheckCollision();
            player.Update();
            foreach (Tile t in tiles) t.Update(player);
            background.Update(player.position, player.speedX / 3, player.speedY / 3);
            foreground.Update(player.position, player.speedX / 2, player.speedY / 2);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);
            foreground.Draw(spriteBatch);
            foreach (Tile t in tiles)
            {
                spriteBatch.Draw(t.texture, t.boundary, null, Color.White);
            }
        }

        public void CheckCollision()
        {
            foreach (Tile t in tiles)
            {
                if (player.boundary.Intersects(t.boundary))
                {
                    if (player.bottom.Intersects(t.boundary))
                    {
                        player.position.Y = t.boundary.Top - player.boundary.Height;
                        player.jumped = false;
                        player.speedY = 0;
                    }
                    else if (player.top.Intersects(t.boundary))
                    {
                        player.position.Y = t.boundary.Bottom;
                        player.speedY = 6;
                    }
                    else if (player.left.Intersects(t.boundary))
                    {
                        player.position.X = t.boundary.Right;
                        player.speedX = 0;
                    }
                    else if (player.right.Intersects(t.boundary))
                    {
                        player.position.X = t.boundary.Left - player.boundary.Width;
                        player.speedX = 0;
                    }
                }
            }
        }
    }
}
