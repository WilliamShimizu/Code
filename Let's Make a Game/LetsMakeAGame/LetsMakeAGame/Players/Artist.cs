using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LetsMakeAGame.Players
{
    class Artist : Player
    {
        List<dot> dots;
        Texture2D dotTexture;
        bool releaseSpecial;

        public Artist()
        {
            dots = new List<dot>();
            dotTexture = Game1.contentMgr.Load<Texture2D>("dot");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            for (int i = 0; i < dots.Count; i++)
            {
                dots[i].Update(this);
            }
            if (releaseSpecial && dots.Count > 0)
            {
                dots[0] = null;
                dots.RemoveAt(0);
            }
            if (dots.Count == 0) releaseSpecial = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            foreach (dot d in dots)
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
            if (dots.Count > 15)
            {
                dots[0] = null;
                dots.RemoveAt(0);
            }
            dots.Add(new dot((int)position.X, (int)position.Y, 5,5, dotTexture));
        }

        public class dot
        {
            private Texture2D texture;
            public Rectangle boundary;
            public dot(int posX, int posY, int width, int height, Texture2D texture)
            {
                boundary = new Rectangle(posX, posY, width, height);
                this.texture = texture;
            }

            public void Update(Player player)
            {
                if (player.boundary.X >= Game1.center.X + 200 || player.boundary.X <= Game1.center.X - 200) boundary.X -= player.speedX;
                if (player.boundary.Y >= Game1.center.Y + 200 || player.boundary.Y <= Game1.center.Y - 200) boundary.Y -= player.speedY;
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(texture, boundary, null, Color.White);
            }
        }
    }
}
