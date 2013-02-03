using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LetsMakeAGame
{
    public class Background
    {
        Texture2D texture;
        Rectangle sourceRect;
        Rectangle destRect;
        Vector2 position;

        public void Initialize(Texture2D texture)
        {
            position = new Vector2(0,(int)(-1*((texture.Height * Game1.scale) - (Game1.center.Y * 2))));
            this.texture = texture;
            sourceRect = new Rectangle(0, 0, (int)texture.Width, (int)texture.Height);
            destRect = new Rectangle((int)position.X, (int)position.Y, (int)(texture.Width * Game1.scale), (int)(texture.Height * Game1.scale));
        }

        public void Update(Rectangle playerPosition, int speedX, int speedY)
        {
            if (playerPosition.X >= Game1.center.X + 200) destRect.X -= speedX;
            if (playerPosition.X <= Game1.center.X - 200) destRect.X -= speedX;
            if (playerPosition.Y >= Game1.center.Y + 200) destRect.Y -= speedY;
            if (playerPosition.Y <= Game1.center.Y - 200) destRect.Y -= speedY;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, destRect, sourceRect, Color.White);
        }
    }
}
