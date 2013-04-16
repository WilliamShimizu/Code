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
                position.Y = other.Top - boundary.Height;
                velocity.Y = 0;
                Update();
                return true;
            }
            else if (bottom.Intersects(other))
            {
                position.Y = other.Top - boundary.Height;
                velocity.Y = 0;
                collidesWithBottom = true;
            }
            else if (top.Intersects(other))
            {
                position.Y = other.Bottom;
                velocity.Y = 4f;
            }
            if (left.Intersects(other))
            {
                position.X = other.Right;
                velocity.X = 0;
            }
            else if (right.Intersects(other))
            {
                position.X = other.Left - boundary.Width;
                velocity.X = 0;
            }
            Update();
            return collidesWithBottom;
        }

        public void Update()
        {
            base.Update();
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
