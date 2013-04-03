using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LetsMakeAGame.Players
{
    class Designer : Player
    {

        public Designer(Rectangle boundary, Texture2D texture)
            : base(boundary, texture, ENTITY.Designer)
        {

        }
        public Cloud cloud;

        public Tile Special()
        {
            if (cloud != null) cloud = null;
            Vector2 v = new Vector2((int)(boundary.X + (boundary.Width * 2)), (int)(boundary.Y - boundary.Height));
            cloud = new Cloud(Game1.cloud, v,this);
            return new Tile(cloud.texture, new Vector2(cloud.boundary.X, cloud.boundary.Y));
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
    public class Cloud : Tile
    {
        public Rectangle boundary;
        public Texture2D texture;
        public int endPointY;
        public const int MAX_HEIGHT = 200;
        public int start;
        public int msElapsed;

        public bool isActive;

        /// <summary>
        /// Constructs the cloud object
        /// </summary>
        /// <param name="player">uses the player's position as a reference for the spawn point.</param>
        public Cloud(Texture2D texture, Vector2 position, Player player) : base(texture, position)
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
            if (player.boundary.X >= Game1.center.X + 200 || player.boundary.X <= Game1.center.X - 200) boundary.X -= player.speedX;
            if (this.boundary.Y <= endPointY) isActive = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.boundary, null, Color.White);
        }

    }
}
