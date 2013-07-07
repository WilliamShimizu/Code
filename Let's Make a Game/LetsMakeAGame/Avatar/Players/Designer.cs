using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Avatar.Players
{
    public class Designer : Player
    {
        public Designer(Rectangle boundary, Texture2D texture)
            : base(boundary, texture, Common.Entity.ENTITY.Designer)
        {

        }

        public Cloud cloud;
        public Texture2D cloudTexture;

        public void Special()
        {
            if (cloud != null) cloud = null;
            Vector2 v = new Vector2((int)(boundary.X + (boundary.Width * 2)), (int)(boundary.Y - boundary.Height));
            cloud = new Cloud(cloudTexture, v, this);
        }

        public void Update(GameTime gameTime)
        {
            base.Update();
            if (cloud != null)
            {
                if (cloud.isActive) cloud.Update(this, gameTime);
                else cloud = null;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (cloud != null) cloud.Draw(spriteBatch);
        }
    }

    /// <summary>
    /// Cloud that the designer can create.
    /// </summary>
    public class Cloud : Common.Entity
    {
        public int endPointY;
        public const int MAX_HEIGHT = 200;
        public int start;
        public int msElapsed;

        public bool isActive;

        /// <summary>
        /// Constructs the cloud object
        /// </summary>
        /// <param name="player">uses the player's position as a reference for the spawn point.</param>
        public Cloud(Texture2D texture, Vector2 position, Player player)
            : base(new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height), texture, ENTITY.Cloud)
        {
            this.texture = texture;
            this.boundary = new Rectangle((int)(position.X), (int)(position.Y), texture.Width, texture.Height);
            endPointY = this.boundary.Y - MAX_HEIGHT;
            isActive = true;
            msElapsed = 0;
        }

        public void Update(Player player, GameTime gameTime)
        {
            msElapsed += gameTime.ElapsedGameTime.Milliseconds;
            this.boundary.Y -= 2;
            if (this.boundary.Y <= endPointY) isActive = false;
        }
    }
}
