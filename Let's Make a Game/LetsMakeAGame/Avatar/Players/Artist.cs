using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Avatar.Players
{
    public class Artist : Player
    {
        public Artist(Rectangle boundary, Texture2D texture)
            : base(boundary, texture, ENTITY.Artist)
        {

        }
    }
}
