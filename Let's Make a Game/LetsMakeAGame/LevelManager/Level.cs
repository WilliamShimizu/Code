using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avatar;
using Avatar.Players;
using eContentManager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelManager
{
    public class Level
    {
        public Artist player;
        public eContentManager.eContentManager contentManager;
        Texture2D background;

        public Level(eContentManager.eContentManager cm)
        {
            this.contentManager = cm;
            player = new Artist(new Microsoft.Xna.Framework.Rectangle(500, 500, 40, 40), cm.getTexture("Tiles/Block"));
            background = cm.getTexture("background");
        }

        public void Update(GameTime gameTime, int x, int y)
        {
            player.Update(x, y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            player.Draw(spriteBatch);
        }

        public Player getActivePlayer()
        {
            return player;
        }
    }
}
