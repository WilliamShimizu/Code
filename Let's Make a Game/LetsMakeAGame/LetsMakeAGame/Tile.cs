using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using LetsMakeAGame.Players;

namespace LetsMakeAGame
{
    public class Tile
    {
        public Texture2D texture;
        int width { get; set; }
        int height { get; set; }
        public Rectangle boundary;
        public bool isLethal;
        public bool isBreakable;
        public Vector2 position;

        //Need to figure out how to reconfigure the grid based on screen size / scale.
        /// <summary>
        /// A tile that will be used to map the level graphically and collision-wise.
        /// </summary>
        /// <param name="level">The level that the tiles will be used in</param>
        /// <param name="tileNumRep">Number representation for the tile that will translate into a texture name</param>
        /// <param name="position">The position at which the texture will be drawn</param>
        public Tile(List<Texture2D> textures, string tileNumRep, Vector2 position)
        {
            foreach (Texture2D txtr in textures)
            {
                if ((string)(Game1.textureLookupTable[txtr.Name.Replace("Tiles/","")]) == tileNumRep)
                {
                    this.texture = txtr;
                    break;
                }
            }
            Vector2 pos = new Vector2((int)(position.X * texture.Width * Game1.scale), (int)(position.Y * texture.Height * Game1.scale));
            this.position = pos;
            this.boundary = Game1.getRect(position, this.texture);
            //this.boundary = new Rectangle((int)pos.X, (int)pos.Y, (int)(this.texture.Width * Game1.scale), (int)(this.texture.Height * Game1.scale));
        }

        public Tile(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.boundary = Game1.getRect(position, texture);
        }

        public void Update(Player player)
        {
            if (player.boundary.X >= Game1.center.X + 200 || player.boundary.X <= Game1.center.X - 200) boundary.X -= player.speedX;
            if (player.boundary.Y >= Game1.center.Y + 200 || player.boundary.Y <= Game1.center.Y - 200) boundary.Y -= player.speedY;
            position.X = boundary.X;
            position.Y = boundary.Y;
        }

        public void Update(int speedX, int speedY)
        {
            boundary.X -= speedX;
            boundary.Y -= speedY;
            position.X = boundary.X;
            position.Y = boundary.Y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, boundary, null, Color.White);
        }

        /// <summary>
        /// Returns an exact copy of this tile.
        /// </summary>
        /// <returns></returns>
        public Tile Copy()
        {
            return new Tile(this.texture, new Vector2(this.boundary.X, this.boundary.Y));
        }

        /// <summary>
        /// Returns a copy of this tile with the given coordinates.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Tile Copy(Vector2 position)
        {
            return new Tile(this.texture, position);
        }
    }
}
