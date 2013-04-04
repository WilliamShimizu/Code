using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avatar.Players
{
    public class QA : Player
    {
        public QA(Microsoft.Xna.Framework.Rectangle boundary, Microsoft.Xna.Framework.Graphics.Texture2D texture)
            : base(boundary, texture, ENTITY.QA)
        {

        }
    }
}
