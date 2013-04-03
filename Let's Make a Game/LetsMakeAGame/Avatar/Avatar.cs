using System;
using System.Collections.Generic;
using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Avatar
{
    public class Avatar : Entity
    {
        public int speedX { get; set; }
        public int speedY { get; set; }

        public Avatar(Rectangle boundary, Texture2D texture, ENTITY type)
            : base(boundary, texture, type)
        {

        }
    }
}
