using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Avatar.Players
{
    public class Engineer : Player
    {
        public List<EngineeringBlock> blocks;

        public Engineer(Rectangle boundary, Texture2D texture)
            : base(boundary, texture, ENTITY.Engineer)
        {
            blocks = new List<EngineeringBlock>();
        }

        new public void Update()
        {
            base.Update();
            foreach (EngineeringBlock b in blocks)
            {
                b.velocity.Y = 6f;
                b.Update();
            }
        }

        new public void Special()
        {
            Rectangle r = new Rectangle(boundary.X + 100, boundary.Y - 60, 40, 40);
            EngineeringBlock b = new EngineeringBlock(r, texture);
            blocks.Add(b);
        }

        public class EngineeringBlock : Common.Collidable
        {

            public bool collidesWithTile { get; set; }

            public EngineeringBlock(Rectangle boundary, Texture2D texture)
            : base(boundary, texture, ENTITY.Tile)
            {

            }
        }
    }
}
