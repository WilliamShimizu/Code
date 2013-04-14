using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Avatar;
using Avatar.Players;
using eContentManager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileManager;
using Common;

namespace LevelManager
{
    public class Level
    {
        public Player player;
        public eContentManager.eContentManager contentManager;
        Texture2D background;
        public List<Player> players;
        public List<Tile> tiles { get; set; }
        private int activePlayerNumber;

        List<Engineer.EngineeringBlock> blocks;

        public Level(eContentManager.eContentManager cm, string mapPath)
        {
            this.contentManager = cm;
            player = new Artist(new Microsoft.Xna.Framework.Rectangle(500, 500, 40, 40), cm.getTexture("Tiles/Block"));
            background = cm.getTexture("background");
            players = new List<Player>();
            activePlayerNumber = 0;
            loadXML(mapPath);
            player = players[activePlayerNumber];
            player.isActive = true;
            foreach (Player p in players)
            {
                //if (p is Artist) dots = ((Artist)p).dots;
                if (p is Engineer) blocks = ((Engineer)p).blocks;
            }
        }

        private void loadXML(string mapPath)
        {
            tiles = new List<Tile>();
            players = new List<Player>();
            XmlReader reader = XmlReader.Create(mapPath);
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
                tiles.Add(new Tile(contentManager.getTexture(name), new Vector2(boundary.X, boundary.Y)));
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
                boundary.X = readInt(reader, "positionX");
                boundary.Y = readInt(reader, "positionY");
                boundary.Height = readInt(reader, "height");
                boundary.Width = readInt(reader, "width");
                name = readString(reader, "textureName");
                reader.ReadEndElement();
                Player player;
                Texture2D texture = contentManager.getTexture(name);
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
                        player = new QA(boundary, texture);
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

        public void Update(int x, int y)
        {
            player.Update(x, y);
            foreach (Player p in players)
            {
                if (!p.Equals(player)) p.Update(0, 0);
            }
            foreach (Engineer.EngineeringBlock block in blocks)
            {
                block.Update(0, 6);
            }
            bool canJump = false;
            
            foreach (Tile t in tiles)
            {
                foreach (Player p in players)
                {
                    if (p.boundary.Intersects(t.boundary))
                    {
                        bool b = p.BasicCollision(t.boundary);
                        if (!canJump && p.Equals(player))
                        {
                            if (b) canJump = true;
                        }
                    }
                }
                foreach (Engineer.EngineeringBlock block in blocks)
                {
                    if (block.boundary.Intersects(t.boundary))
                    {
                        block.collidesWithTile = block.BasicCollision(t.boundary);
                    }
                }
            }
            foreach (Engineer.EngineeringBlock block in blocks)
            {
                foreach (Engineer.EngineeringBlock b in blocks)
                {
                    if (block.boundary.Intersects(b.boundary)) block.BasicCollision(b.boundary);
                }
                foreach (Player p in players)
                {
                    if (block.boundary.Intersects(p.boundary))
                    {
                        //if (!p.BasicCollision(block.boundary)) 
                        block.BasicCollision(p.boundary);
                    }
                }
            }
            player.canJump = canJump;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(background != null) spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            foreach (Tile t in tiles) t.Draw(spriteBatch);
            foreach(Player p in players) p.Draw(spriteBatch);
            foreach (Engineer.EngineeringBlock block in blocks) block.Draw(spriteBatch);
        }

        public Player getActivePlayer()
        {
            return player;
        }

        public void switchPlayers()
        {
            player.isActive = false;
            activePlayerNumber++;
            if (activePlayerNumber > players.Count - 1) activePlayerNumber = 0;
            player = players[activePlayerNumber];
            player.isActive = true;
        }

        public bool isWithin(Rectangle obj, Vector2 cursorPosition)
        {
            return (cursorPosition.X >= obj.X && cursorPosition.X <= obj.X + obj.Width && cursorPosition.Y >= obj.Y && cursorPosition.Y <= obj.Y + obj.Height);
        }
    }
}
