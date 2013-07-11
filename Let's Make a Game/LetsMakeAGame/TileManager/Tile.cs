using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileManager
{
    public class Tile : Entity
    {
        public bool isDestructable { get; set; }

        public Tile(Texture2D texture, Vector2 position)
            : base(new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height), texture, ENTITY.Tile)
        {

        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Tile)) return false;
            Tile t = (Tile)obj;
            return (t.boundary.X == boundary.X && t.boundary.Y == boundary.Y && t.fileName == fileName);
        }

        public Tile Copy(Vector2 position)
        {
            return new Tile(texture, position);
        }
    }
}
