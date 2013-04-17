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
        //public Rectangle bottom;
        public Rectangle left;
        public Rectangle right;
        public HashSet<CollisionType> collisions;

        private const int BUFFER = 7;
        private const int DIST = 3;

        public Collidable(Rectangle boundary, Texture2D texture, ENTITY type)
            : base(boundary, texture, type)
        {
            CreateCollisionRectangles();
            collisions = new HashSet<CollisionType>();
        }

        private void CreateCollisionRectangles()
        {
            top = new Rectangle(boundary.X + BUFFER, boundary.Top - DIST, boundary.Width - (BUFFER * 2), 1);
            left = new Rectangle(boundary.Left - DIST, boundary.Y + BUFFER, 1, boundary.Height - (BUFFER * 2));
            right = new Rectangle(boundary.Right + DIST, boundary.Y + BUFFER, 1, boundary.Height - (BUFFER * 2));
        }

        public enum CollisionType
        {
            bottom,
            top,
            left,
            right
        }

        public bool BasicCollision(Rectangle other)
        {
            bool collidesWithBottom = false;
            bool collidesWithWall = false;
            bool collidesWithTop = false;
            
            if (left.Intersects(other) && velocity.Y < 5f)
            {
                position.X = other.Right;
                velocity.X = 0;
                collidesWithWall = true;
            }
            else if (right.Intersects(other) && velocity.Y < 5f)
            {
                position.X = other.Left - boundary.Width;
                velocity.X = 0;
                collidesWithWall = true;
            }
            if (top.Intersects(other))// && !collidesWithWall)
            {
                position.Y = other.Bottom;
                velocity.Y = 2f;
            }
            else if(boundary.Bottom > other.Top && !(collidesWithWall || collidesWithTop))
            {
                position.Y = other.Top - boundary.Height;
                velocity.Y = 0;
                collidesWithBottom = true;
            }
            Update();
            return collidesWithBottom;
        }

        public void Update()
        {
            base.Update();
            CreateCollisionRectangles();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(texture, top, null, Color.Red);
            spriteBatch.Draw(texture, left, null, Color.Red);
            spriteBatch.Draw(texture, right, null, Color.Red);
        }
    }
}
