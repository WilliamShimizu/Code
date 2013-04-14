using System;
using System.Collections.Generic;
using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Avatar
{
    public abstract class Avatar : Collidable
    {
        private const int JUMP_HEIGHT = 80;
        private const int PLAYER_MOVE_SPEED = 6;
        public bool canJump;
        private Vector2 jumpPoint;
        private bool jumped;

        public Avatar(Rectangle boundary, Texture2D texture, ENTITY type)
            : base(boundary, texture, type)
        {
            canJump = true;
        }

        new public void Update(int speedX, int speedY)
        {
            if (jumped)
            {
                canJump = false;
                if (!(jumpPoint.Y == 0 && jumpPoint.X == 0) && boundary.Y >= jumpPoint.Y - JUMP_HEIGHT)
                {
                    speedY = -PLAYER_MOVE_SPEED;
                }
                else
                {
                    speedY = PLAYER_MOVE_SPEED;
                    jumped = false;
                }
            }
            else speedY = PLAYER_MOVE_SPEED;
            base.Update(speedX, speedY);
        }

        public void Jump()
         {
             if (canJump)
             {
                 jumpPoint = new Vector2(boundary.X, boundary.Y);
                 jumped = true;
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
