using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Avatar.Players
{
    public class Musician : Player
    {
        public Musician(Rectangle boundary, Texture2D texture)
            : base(boundary, texture, Common.Entity.ENTITY.Musician)
        {

        }
    }
}
