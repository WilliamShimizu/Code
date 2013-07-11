using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Common;

namespace UI
{
    class Prompt : Entity
    {
        private string header;
        private string message;
        private List<Button> buttons;
        private Vector2 position;
        private Texture2D texture;

        public Prompt(string header, string message, Texture2D texture) : base(new Rectangle(0, 0, texture.Width, texture.Height), "", ENTITY.UI)
        {
            this.header = header;
            this.message = message;
            this.texture = texture;
            int x = (Globals.viewport.Width / 2) - (texture.Width / 2);
            int y = (Globals.viewport.Height / 2) - (texture.Height / 2);
            this.position = new Vector2(x, y);
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.DrawString(Globals.font, header, new Vector2(position.X + 10, position.Y + 5), Color.White);
            Vector2 v = new Vector2(position.X + (texture.Width / 2), position.Y + (texture.Height / 2));
            spriteBatch.DrawString(Globals.font, message, v, Color.White);
        }
    }
}
