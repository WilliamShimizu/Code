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

namespace LevelManager
{
    public class Level
    {
        public Player player;
        public eContentManager.eContentManager contentManager;
        Texture2D background;
        public List<Player> players;
        List<Tile> tiles;
        private int activePlayerNumber;

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

        public void Update(int x, int y)
        {
            player.Update(x, y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(background != null) spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            player.Draw(spriteBatch);
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
    }
}
