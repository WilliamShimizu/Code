using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LetsMakeAGame.Players
{
    public class Player
    {
        private const int JUMP_HEIGHT = 80;
        private const int PLAYER_MOVE_SPEED = 6;

        private int ground;
        public int speedX { get; set; }
        public int speedY { get; set; }

        public bool jumped { get; set; }

        //public Vector2 position;
        private Vector2 jumpPoint;

        public Rectangle boundary;
        public Rectangle top;
        public Rectangle bottom;
        public Rectangle left;
        public Rectangle right;

        public bool gravityIsOn = true;
        public bool canJump;

        private Texture2D texture;

        private Viewport view;
       
        public void Initialize(Texture2D texture, Vector2 position, Viewport view)
        {
            this.texture = texture;
            //this.position = position;
            this.view = view;
            //set collision rectangle
            boundary = new Rectangle((int)position.X, (int)position.Y, (int)(texture.Width * Game1.scale), (int)(texture.Height * Game1.scale));
            top = new Rectangle(boundary.X + 7, boundary.Y - 7, boundary.Width - 14, 1);
            bottom = new Rectangle(boundary.X + 7, boundary.Y + boundary.Height + 7, boundary.Width - 14, 1);
            left = new Rectangle(boundary.X - 7, boundary.Y + 7, 1, boundary.Height - 14);
            right = new Rectangle(boundary.X + boundary.Width + 7, boundary.Y + 7, 1, boundary.Height - 14);
            jumped = false;
            ground = (int)Game1.center.Y + 200;
        }

        public virtual void Update(GameTime gameTime)
        {
            //Update Position
            if (jumped)
            {
                if (boundary.Y <= jumpPoint.Y - JUMP_HEIGHT)
                {
                    speedY = PLAYER_MOVE_SPEED;
                }
            }
            else if (gravityIsOn) speedY = PLAYER_MOVE_SPEED;
            boundary.X += speedX;
            boundary.Y += speedY;
            //Make sure the player stays within a certain part of the screen.
            if (boundary.X >= Game1.center.X + 200) boundary.X = (int)Game1.center.X + 200;
            if (boundary.X <= Game1.center.X - 200) boundary.X = (int)Game1.center.X - 200;
            if (boundary.Y >= Game1.center.Y + 200) boundary.Y = (int)Game1.center.Y + 200;
            if (boundary.Y <= Game1.center.Y - 200) boundary.Y = (int)Game1.center.Y - 200;
            //Set Collision Boundaries
            top.X = boundary.X + 7;
            top.Y = boundary.Y - 7;
            bottom.X = boundary.X + 7;
            bottom.Y = boundary.Y + boundary.Height + 7;
            left.X = boundary.X - 7;
            left.Y = boundary.Y + 7;
            right.X = boundary.X + boundary.Width + 7;
            right.Y = boundary.Y + 7;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //using the overload of Draw that requires a destination and source rectangle. We can keep the source as is
            //and scale the destination rectangle as needed (once we implement scaling).
            spriteBatch.Draw(texture, boundary, null, Color.White);
            spriteBatch.Draw(new Texture2D(Game1.gd, top.Width, top.Height), top, null, Color.Red);
            spriteBatch.Draw(new Texture2D(Game1.gd, bottom.Width, bottom.Height), bottom, null, Color.Red);
            spriteBatch.Draw(new Texture2D(Game1.gd, left.Width, left.Height), left, null, Color.Red);
            spriteBatch.Draw(new Texture2D(Game1.gd, right.Width, right.Height), right, null, Color.Red);
            
        }

        public void Jump(int moveSpeed)
        {
            speedY = -moveSpeed;
            jumped = true;
            jumpPoint = new Vector2(boundary.X, boundary.Y);
        }

        public virtual void Special()
        {

        }
    }
}
