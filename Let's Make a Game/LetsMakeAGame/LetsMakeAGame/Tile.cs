using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LetsMakeAGame
{
    public class Tile
    {
        public Texture2D texture;
        int width { get; set; }
        int height { get; set; }
        public Rectangle boundary;

        //Need to figure out how to reconfigure the grid based on screen size / scale.
        public Tile(string tileNumRep, Vector2 position)
        {
            switch (tileNumRep)
            {
                case "1":
                    texture = Game1.stone;
                    break;
                case "2":
                    texture = Game1.leopard;
                    break;
                case "3":
                    texture = Game1.stars;
                    break;
                default:
                    break;
            }
            Vector2 pos = new Vector2((int)(position.X * texture.Width * Game1.scale), (int)(position.Y * texture.Height * Game1.scale));
            this.boundary = new Rectangle((int)pos.X, (int)pos.Y, (int)(this.texture.Width * Game1.scale), (int)(this.texture.Height * Game1.scale));
        }

        public void Update(Player player)
        {
            if (player.position.X >= Game1.center.X + 200) boundary.X -= player.speedX;
            if (player.position.X <= Game1.center.X - 200) boundary.X -= player.speedX;
            if (player.position.Y >= Game1.center.Y + 200) boundary.Y -= player.speedY;
            if (player.position.Y <= Game1.center.Y - 200) boundary.Y -= player.speedY;
        }
    }
}
