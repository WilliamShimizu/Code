using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Common
{
    public class Collidable : Entity
    {
        public Rectangle top;
        public Rectangle bottom;
        public Rectangle left;
        public Rectangle right;
        public int speedX { get; set; }
        public int speedY { get; set; }

        private const int RECT_LEN = 7;

        public Collidable(Rectangle boundary, Texture2D texture, ENTITY type)
            : base(boundary, texture, type)
        {
            top = new Rectangle(boundary.X + RECT_LEN, boundary.Y - RECT_LEN, boundary.Width - RECT_LEN * 2, 1);
            bottom = new Rectangle(boundary.X + RECT_LEN, boundary.Y + boundary.Height + RECT_LEN, boundary.Width - RECT_LEN * 2, 1);
            left = new Rectangle(boundary.X - RECT_LEN, boundary.Y + RECT_LEN, 1, boundary.Height - RECT_LEN * 2);
            right = new Rectangle(boundary.X + boundary.Width + RECT_LEN, boundary.Y + RECT_LEN, 1, boundary.Height - RECT_LEN * 2);
        }

        private void BasicCollision(Rectangle other)
        {
            if (bottom.Intersects(other) && (left.Intersects(other) || right.Intersects(other)))
            {
                boundary.Y = other.Top - boundary.Height;
                speedY = 0;
                return;
            }
            if (bottom.Intersects(other))
            {
                boundary.Y = other.Top - boundary.Height;
                speedY = 0;
            }
            if (top.Intersects(other))
            {
                boundary.Y = other.Bottom;
                speedY = 6;
            }
            if (left.Intersects(other))
            {
                boundary.X = other.Right;
                speedX = 0;
            }
            if (right.Intersects(other))
            {
                boundary.X = other.Left - boundary.Width;
                speedX = 0;
            }
        }
    }
}
