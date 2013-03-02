using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LetsMakeAGame;

namespace LetsMakeAGame.UI
{
    class Prompt
    {
        private string header;
        private string message;
        private List<Button> buttons;
        private Vector2 position;
        private Texture2D texture;

        public Prompt(string header, string message, Texture2D texture)
        {
            this.header = header;
            this.message = message;
            this.texture = texture;
            int x = (Game1.viewport.Width / 2) - (texture.Width / 2);
            int y = (Game1.viewport.Height / 2) - (texture.Height / 2);
            this.position = new Vector2(x, y);
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.DrawString(Game1.font, header, new Vector2(position.X + 10, position.Y + 5), Color.White);
            Vector2 v = new Vector2(position.X + (texture.Width / 2), position.Y + (texture.Height / 2));
            spriteBatch.DrawString(Game1.font, message, v, Color.White);
        }
    }
}
