using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LetsMakeAGame
{
    class Engineer : Player
    {
        public List<EngineeringBlock> blocks;

        public override void Special()
        {
            blocks.Add(new EngineeringBlock(this));
            if (blocks.Count > 5)
            {
                blocks[0] = null;
                blocks.RemoveAt(0);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (blocks == null)
            {
                blocks = new List<EngineeringBlock>();
                blocks.Capacity = 5;
            }
            foreach (EngineeringBlock block in blocks)
            {
                block.Update(this);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            foreach (EngineeringBlock block in blocks)
            {
                spriteBatch.Draw(block.texture, block.boundary, null, Color.White);
            }
        }
    }

    public class EngineeringBlock
    {
        public Level level;
        public Texture2D texture;
        public Rectangle boundary;
        public int currentNumberOfBlocks;
        
        public EngineeringBlock(Player player)
        {
            this.texture = Game1.engineeringBlock;
            this.boundary = new Rectangle((int)(player.boundary.X + (player.boundary.Width * 2)), (int)(player.boundary.Y - player.boundary.Height), texture.Width, texture.Height);
        }

        public void Update(Player player)
        {
            this.boundary.Y += 6;
            if (player.boundary.X >= Game1.center.X + 200 || player.boundary.X <= Game1.center.X - 200) boundary.X -= player.speedX;
            if (player.boundary.Y >= Game1.center.Y + 200 || player.boundary.Y <= Game1.center.Y - 200) boundary.Y -= player.speedX;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.boundary, null, Color.White);
        }
    }

}
