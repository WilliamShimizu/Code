using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Common
{
    public abstract class Collidable : Entity
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

        public bool BasicCollision(Rectangle other)
        {
            bool collidesWithBottom = false;
            if (bottom.Intersects(other) && (left.Intersects(other) || right.Intersects(other)))
            {
                boundary.Y = other.Top - boundary.Height;
                speedY = 0;
                return true;
            }
            else if (bottom.Intersects(other))
            {
                boundary.Y = other.Top - boundary.Height;
                speedY = 0;
                collidesWithBottom = true;
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
            Update(0, 0);
            return collidesWithBottom;
        }

        public void Update(int x, int y)
        {
            base.Update(x, y);
            top.X = boundary.X + RECT_LEN;
            top.Y = boundary.Y - RECT_LEN;
            bottom.X = boundary.X + RECT_LEN;
            bottom.Y = boundary.Y + boundary.Height + RECT_LEN;
            left.X = boundary.X - RECT_LEN;
            left.Y = boundary.Y + RECT_LEN;
            right.X = boundary.X + boundary.Width + RECT_LEN;
            right.Y = boundary.Y + RECT_LEN;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(texture, top, null, Color.Red);
            spriteBatch.Draw(texture, bottom, null, Color.Red);
            spriteBatch.Draw(texture, left, null, Color.Red);
            spriteBatch.Draw(texture, right, null, Color.Red);
        }
    }
}
