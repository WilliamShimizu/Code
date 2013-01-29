using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace LetsMakeAGame
{
    class Level
    {
        public Background background;
        public Background foreground;
        public static List<Tile> tiles;
        List<string> lines;

        public Level()
        {
            tiles = new List<Tile>();
            lines = new List<string>();
            StreamReader sr = new StreamReader("Content/Maps/testMap.txt");
            while (true)
            {
                string line = sr.ReadLine();
                if (line == null)
                {
                    sr.Close();
                    break;
                }
                lines.Add(line);
            }
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
    }
}
