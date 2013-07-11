using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Avatar
{
    public class Player : Avatar
    {


        public bool isActive { get; set; }

        public Player(Rectangle boundary, Texture2D texture, ENTITY type)
            : base(boundary, texture, type)
        {

        }

        public void Special()
        {

        }

        public Player Copy(Vector2 position)
        {
            return new Player(new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height), texture, type);
        }
    }
}
