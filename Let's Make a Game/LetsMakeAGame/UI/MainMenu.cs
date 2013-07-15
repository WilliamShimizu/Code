using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Common;
using System.IO;

namespace UI
{
    public class MainMenu
    {
        SpriteFont font;
        string levelDir;
        Rectangle boundary;
        List<Button> buttons;
        Vector2 position;

        public MainMenu(string levelDirectory)
        {
            font = Globals.font;
            levelDir = levelDirectory;
            buttons = new List<Button>();
            foreach (string s in Directory.GetFiles(levelDirectory))
            {
                string levelName = s.Remove(0, s.LastIndexOf("\\") + 1);
                buttons.Add(new Button(Globals.contentManager.getTexture("Buttons/Button"), levelName));
            }
            buttons.Add(new Button(Globals.contentManager.getTexture("Buttons/Button"), "Edit Map"));
            boundary = new Rectangle();
            boundary.Width = Globals.viewport.Width - 200;
            boundary.Height = Globals.viewport.Height - 200;
            boundary.X = 100;
            boundary.Y = 100;
            position = new Vector2(boundary.X, boundary.Y);
        }

        public void Update(GameTime gameTime)
        {
            int x = (int)this.position.X + 10;
            int y = (int)this.position.Y + 10;
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].boundary.X = x;// + ((i % 2) * 80);
                buttons[i].boundary.Y = y + (80 * i);// + ((i / 2) * 80);
                buttons[i].Update(gameTime, 0, 0);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(Globals.contentManager.getTexture("crappyMenu"), boundary, Color.White);
            foreach (Button b in buttons)
            {
                b.Draw(spriteBatch);
            }
        }
    }
}
