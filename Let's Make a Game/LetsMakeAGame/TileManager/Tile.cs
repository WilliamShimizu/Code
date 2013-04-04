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
        public Tile(Texture2D texture, Vector2 position)
            : base(new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height), texture, ENTITY.Tile)
        {
            this.texture = texture;
            this.boundary = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }
    }
}
