using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LetsMakeAGame.Players
{
    class Artist : Player
    {
        private const int DOT_LENGTH = 5;
        public List<Dot> dots;
        Texture2D dotTexture;
        bool releaseSpecial;

        public Artist()
        {
            dots = new List<Dot>();
            dotTexture = Game1.contentMgr.Load<Texture2D>("dot");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            for (int i = 0; i < dots.Count; i++)
            {
                dots[i].Update(this);
            }
            //if (releaseSpecial && dots.Count > 0)
            //{
            //    dots[0] = null;
            //    dots.RemoveAt(0);
            //}
            if (dots.Count == 0) releaseSpecial = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            foreach (Dot d in dots)
            {
                d.Draw(spriteBatch);
            }
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
    }
    public class Dot
    {
        private Texture2D texture;
        public Rectangle boundary;
        private Rectangle textureBoundary;
        public Dot(int posX, int posY, int width, int height, Texture2D texture)
        {
            textureBoundary = new Rectangle(posX, posY, width, height);
            boundary = new Rectangle(posX, posY, width + 10, height + 10);
            this.texture = texture;
        }

        public void Update(Player player)
        {
            if (player.boundary.X >= Game1.center.X + 200 || player.boundary.X <= Game1.center.X - 200)
            {
                boundary.X -= player.speedX;
                textureBoundary.X -= player.speedX;
            }
            if (player.boundary.Y >= Game1.center.Y + 200 || player.boundary.Y <= Game1.center.Y - 200)
            {
                boundary.Y -= player.speedY;
                textureBoundary.Y -= player.speedY;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, textureBoundary, null, Color.White);
        }
    }
}
