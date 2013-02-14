using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LetsMakeAGame.Players
{
    class Designer : Player
    {
        public Cloud cloud;

        public override void Special()
        {
            if (cloud != null) cloud = null;
            cloud = new Cloud(this);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (cloud != null)
            {
                if (cloud.isActive) cloud.Update(this, gameTime);
                else cloud = null;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (cloud != null) cloud.Draw(spriteBatch);
        }
    }

    /// <summary>
    /// Cloud that the designer can create.
    /// </summary>
    public class Cloud
    {
        public Rectangle boundary;
        private Texture2D texture;
        public int endPointY;
        public const int MAX_HEIGHT = 200;
        public int start;

        public bool isActive;

        /// <summary>
        /// Constructs the cloud object
        /// </summary>
        /// <param name="player">uses the player's position as a reference for the spawn point.</param>
        public Cloud(Player player)
        {
            this.texture = Game1.cloud;
            this.boundary = new Rectangle((int)(player.boundary.X + (player.boundary.Width * 2)), (int)(player.boundary.Y - player.boundary.Height), texture.Width, texture.Height);
            endPointY = this.boundary.Y - MAX_HEIGHT;
            isActive = true;
        }

        public void Update(Player player, GameTime gameTime)
        {
            this.boundary.Y -= 2;
            if (player.boundary.X >= Game1.center.X + 200 || player.boundary.X <= Game1.center.X - 200) boundary.X -= player.speedX;
            if (this.boundary.Y <= endPointY) isActive = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.boundary, null, Color.White);
        }

    }
}
