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

        private bool canJump;
        private Vector2 jumpPoint;
        public bool isActive { get; set; }

        public Player(Rectangle boundary, Texture2D texture, ENTITY type)
            : base(boundary, texture, type)
        {

        }

        public void move(int speedX, int speedY)
        {
            this.speedX = speedX;
            this.speedX = speedY;
        }

        public void Jump(int moveSpeed)
        {
            speedY = -moveSpeed;
            canJump = false;
            jumpPoint = new Vector2(boundary.X, boundary.Y);
        }

        public void Special()
        {

        }
    }
}
