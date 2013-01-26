using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LetsMakeAGame
{
    class Player
    {
        private const int JUMP_HEIGHT = 80;
        private const int PLAYER_MOVE_SPEED = 6;

        private int ground;
        public int speedX { get; set; }
        public int speedY { get; set; }
        private float scale;

        public bool jumped { get; set; }

        public Vector2 position;
        private Vector2 jumpPoint;

        public Rectangle boundary;
        private Rectangle sourceRectangle;
        private Texture2D texture;

        private Viewport view;
       
        public void Initialize(Texture2D texture, Vector2 position, Viewport view, float scale)
        {
            this.texture = texture;
            this.position = position;
            this.view = view;
            this.scale = scale;
            //set collision rectangle
            boundary = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            //set source of texture rectangle
            sourceRectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            jumped = false;
            ground = view.TitleSafeArea.Height - boundary.Height;
        }

        public void Update()
        {
            //Update Position
            position.X += speedX;
            position.Y += speedY;
            if (jumped)
            {
                if (position.Y <= jumpPoint.Y - JUMP_HEIGHT)
                {
                    speedY = PLAYER_MOVE_SPEED;
                }
                if (position.Y >= ground)
                {
                    position.Y = ground;
                    jumped = false;
                }
            }
            //Check in-game boundaries
            if (view.TitleSafeArea.Width - position.X <= 100)
            {
                position.X = view.TitleSafeArea.Width - 100;
            }
            if (position.X <= 60)
            {
                position.X = 60;
            }
            if (position.Y + boundary.Height >= view.TitleSafeArea.Height)
            {
                position.Y = view.TitleSafeArea.Height - boundary.Height;
            }
            if (position.Y <= 60)
            {
                position.Y = 60;
            }
            //Set Collision Boundary
            boundary.X = (int)position.X;
            boundary.Y = (int)position.Y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //using the overload of Draw that requires a destination and source rectangle. We can keep the source as is
            //and scale the destination rectangle as needed (once we implement scaling).
            spriteBatch.Draw(texture, boundary, sourceRectangle, Color.Gray);
        }

        public void Jump(int moveSpeed)
        {
            speedY = moveSpeed;
            jumped = true;
            jumpPoint = position;
        }
    }
}
