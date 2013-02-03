using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LetsMakeAGame
{
    public class Player
    {
        private const int JUMP_HEIGHT = 80;
        private const int PLAYER_MOVE_SPEED = 6;

        private int ground;
        public int speedX { get; set; }
        public int speedY { get; set; }

        public bool jumped { get; set; }

        public Vector2 position;
        private Vector2 jumpPoint;

        public Rectangle boundary;
        public Rectangle top;
        public Rectangle bottom;
        public Rectangle left;
        public Rectangle right;

        private Texture2D texture;

        private Viewport view;
       
        public void Initialize(Texture2D texture, Vector2 position, Viewport view)
        {
            this.texture = texture;
            this.position = position;
            this.view = view;
            //set collision rectangle
            boundary = new Rectangle((int)position.X, (int)position.Y, (int)(texture.Width * Game1.scale), (int)(texture.Height * Game1.scale));
            //set source of texture rectangle
            //sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            top = new Rectangle(boundary.X + 1, boundary.Y - 1, boundary.Width - 2, 1);
            bottom = new Rectangle(boundary.X + 1, boundary.Y + boundary.Height + 1, boundary.Width - 2, 1);
            left = new Rectangle(boundary.X - 1, boundary.Y + 1, 1, boundary.Height - 2);
            right = new Rectangle(boundary.X + boundary.Width + 1, boundary.Y + 1, 1, boundary.Height - 2);
            jumped = false;
            ground = (int)Game1.center.Y + 200;
        }

        public void Update()
        {
            //Update Position
            
            
            if (jumped)
            {
                if (position.Y <= jumpPoint.Y - JUMP_HEIGHT)
                {
                    speedY = PLAYER_MOVE_SPEED;
                }
            }
            //else speedY = 6;
            position.X += speedX;
            position.Y += speedY;
            //Make sure the player stays within a certain part of the screen.
            if (position.X >= Game1.center.X + 200) position.X = Game1.center.X + 200;
            if (position.X <= Game1.center.X - 200) position.X = Game1.center.X - 200;
            if (position.Y >= Game1.center.Y + 200) position.Y = Game1.center.Y + 200;
            if (position.Y <= Game1.center.Y - 200) position.Y = Game1.center.Y - 200;
            //Set Collision Boundaries
            boundary.X = (int)position.X;
            boundary.Y = (int)position.Y;
            top.X = boundary.X + 1;
            top.Y = boundary.Y - 1;
            left.X = boundary.X - 1;
            left.Y = boundary.Y + 1;
            bottom.X = boundary.X + 1;
            bottom.Y = boundary.Y + boundary.Height + 1;
            right.X = boundary.X + boundary.Width + 1;
            right.Y = boundary.Y + 1;
        }

        public void Draw(SpriteBatch spriteBatch)
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
            jumpPoint = position;
        }
    }
}
