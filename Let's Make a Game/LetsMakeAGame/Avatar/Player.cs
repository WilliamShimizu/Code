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
        public Player(Rectangle boundary, Texture2D texture, ENTITY type)
            : base(boundary, texture, type)
        {

        }

        public void move(int speedX, int speedY)
        {
            this.speedX = speedX;
            this.speedX = speedY;
        }
    }
}
