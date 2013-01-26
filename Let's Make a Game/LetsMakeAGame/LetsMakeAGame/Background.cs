using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LetsMakeAGame
{
    class Background
    {
        Texture2D texture;
        //Rectangle destRect;
        Vector2 position;
        Viewport view;
        private float scale;

        public void Initialize(Texture2D texture, Vector2 position, Viewport view, float scale)
        {
            this.position = position;
            this.texture = texture;
            this.view = view;
            this.scale = scale;
        }

        public void Update(Vector2 playerPosition, int speedX, int speedY)
        {
            if (view.TitleSafeArea.Width - playerPosition.X <= 100)
            {
                position.X -= speedX;
            }
            if (playerPosition.X <= 60)
            {
                position.X -= speedX;
            }
            if (playerPosition.Y <= 60)
            {
                position.Y -= speedY;
            }
            if (playerPosition.Y >= (view.TitleSafeArea.Height - 60))
            {
                position.Y -= speedY;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
