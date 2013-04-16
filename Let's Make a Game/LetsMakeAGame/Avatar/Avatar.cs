using System;
using System.Collections.Generic;
using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Avatar
{
    public abstract class Avatar : Collidable
    {
        public bool canJump;

        public Avatar(Rectangle boundary, Texture2D texture, ENTITY type)
            : base(boundary, texture, type)
        {
            canJump = true;
        }

        new public void Update()
        {
            if (!canJump)
            {
                velocity.Y += 0.15f;
                if (velocity.Y > 0) velocity.Y += 0.05f;
            }
            else velocity.Y = 4f;
            base.Update();
        }

        public void Jump()
         {
             if (canJump)
             {
                 velocity.Y = -6f;
                 canJump = false;
             }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Avatar)) return false;
            else
            {
                Avatar a = (Avatar)obj;
                return (type == a.type && boundary.X == a.boundary.X && boundary.Y == a.boundary.Y);
            }
        }
    }
}
