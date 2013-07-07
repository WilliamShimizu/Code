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
        private const int DOT_LENGTH = 5;
        public List<Dot> dots;
        public Texture2D dotTexture;
        bool releaseSpecial;

        public Artist(Rectangle boundary, Texture2D texture)
            : base(boundary, texture, ENTITY.Artist)
        {
            dots = new List<Dot>();
        }

        public void Update()
        {
            base.Update();
            //for (int i = 0; i < dots.Count; i++)
            //{
            //    dots[i].Update(this);
            //}
            //if (releaseSpecial && dots.Count > 0)
            //{
            //    dots[0] = null;
            //    dots.RemoveAt(0);
            //}
            if (dots.Count == 0) releaseSpecial = false;
        }

        public void ReleaseSpecial()
        {
            releaseSpecial = true;
        }

        public void Special(Vector2 position)
        {
            if (releaseSpecial && dots.Count > 0) dots.Clear();
            int x = (int)position.X;
            int y = (int)position.Y;
            if (dots.Count > 0)
            {
                x = dots[dots.Count - 1].boundary.X;
                y = dots[dots.Count - 1].boundary.Y;
                if ((int)position.X > x) x += DOT_LENGTH;
                if ((int)position.X < x) x -= DOT_LENGTH;
                if ((int)position.Y > y) y += DOT_LENGTH;
                if ((int)position.Y < y) y -= DOT_LENGTH;
            }
            if (dots.Count > 40)
            {
                dots[0] = null;
                dots.RemoveAt(0);
            }
            
            dots.Add(new Dot(x, y, DOT_LENGTH, DOT_LENGTH, dotTexture));
            releaseSpecial = false;
        }

        public class Dot : Common.Entity
        {
            public Dot(int posX, int posY, int width, int height, Texture2D texture)
                : base(new Rectangle(posX, posY, width + 10, height + 10), texture, ENTITY.Dot)
            {

            }
        }
    }
}
