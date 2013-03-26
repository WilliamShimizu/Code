using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LetsMakeAGame.Players
{
    public class Player : Common.Entity
    {
        private const int JUMP_HEIGHT = 80;
        private const int PLAYER_MOVE_SPEED = 6;
        public bool isActive { get; set; }

        private int ground;
        public int speedX { get; set; }
        public int speedY { get; set; }


        //public Vector2 position;
        private Vector2 jumpPoint;

        public Rectangle boundary;
        public Rectangle top;
        public Rectangle bottom;
        public Rectangle left;
        public Rectangle right;

        private const int RECT_LEN = 7;

        public bool gravityIsOn = true;
        public bool canJump;

        private Texture2D texture;

        private Viewport view;

        public Player(Rectangle boundary, Texture2D texture, ENTITY type) : base(boundary, texture, type)
        {
            Vector2 v = new Vector2(boundary.X, boundary.Y);
            Initialize(texture, v, Game1.viewport);
        }

        public Player Copy(Vector2 position)
        {
            return new Player(new Rectangle((int)position.X, (int)position.Y, this.boundary.Width, this.boundary.Height), this.texture, this.type);
        }

        public override bool Equals(object obj)
        {
            if(!(obj is Player) || obj == null) return false;
            else
            {
                Player p = (Player)obj;
                return (p.boundary.X == this.boundary.X && p.boundary.Y == this.boundary.Y && this.type == p.type);
            }
        }
       
        public void Initialize(Texture2D texture, Vector2 position, Viewport view)
        {
            this.texture = texture;
            //this.position = position;
            this.view = view;
            //set collision rectangle
            boundary = new Rectangle((int)position.X, (int)position.Y, (int)(texture.Width * Game1.scale), (int)(texture.Height * Game1.scale));
            top = new Rectangle(boundary.X + RECT_LEN, boundary.Y - RECT_LEN, boundary.Width - RECT_LEN * 2, 1);
            bottom = new Rectangle(boundary.X + RECT_LEN, boundary.Y + boundary.Height + RECT_LEN, boundary.Width - RECT_LEN * 2, 1);
            left = new Rectangle(boundary.X - RECT_LEN, boundary.Y + RECT_LEN, 1, boundary.Height - RECT_LEN * 2);
            right = new Rectangle(boundary.X + boundary.Width + RECT_LEN, boundary.Y + RECT_LEN, 1, boundary.Height - RECT_LEN * 2);
            canJump = true;
            ground = (int)Game1.center.Y + 200;
            isActive = false;
        }

        public virtual void Update(GameTime gameTime)
        {
            //Update Position
            if (!canJump)
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
            if (isActive)
            {
                if (boundary.X >= Game1.center.X + 500) boundary.X = (int)Game1.center.X + 500;
                if (boundary.X <= Game1.center.X - 500) boundary.X = (int)Game1.center.X - 500;
                if (boundary.Y >= Game1.center.Y + 500) boundary.Y = (int)Game1.center.Y + 500;
                if (boundary.Y <= Game1.center.Y - 500) boundary.Y = (int)Game1.center.Y - 500;
            }
        }

        public void Update(Player player)
        {
            if (!isActive)
            {
                if (player.boundary.X >= Game1.center.X + 500 || player.boundary.X <= Game1.center.X - 500) boundary.X -= player.speedX;
                if (player.boundary.Y >= Game1.center.Y + 500 || player.boundary.Y <= Game1.center.Y - 500) boundary.Y -= player.speedY;
                boundary.Y += 6;
            }
        }

        public void UpdateCollisionBoundaries(bool tile)
        {
            int subtractValue = tile ? 0 : PLAYER_MOVE_SPEED;
            top.X = boundary.X + RECT_LEN;
            top.Y = boundary.Y - RECT_LEN - subtractValue;
            bottom.X = boundary.X + RECT_LEN;
            bottom.Y = boundary.Y + boundary.Height + RECT_LEN - subtractValue;
            left.X = boundary.X - RECT_LEN;
            left.Y = boundary.Y + RECT_LEN - subtractValue;
            right.X = boundary.X + boundary.Width + RECT_LEN;
            right.Y = boundary.Y + RECT_LEN - subtractValue;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //using the overload of Draw that requires a destination and source rectangle. We can keep the source as is
            //and scale the destination rectangle as needed (once we implement scaling).
            spriteBatch.Draw(texture, boundary, null, Color.White);
            spriteBatch.Draw(texture, top, null, Color.Red);
            spriteBatch.Draw(texture, bottom, null, Color.Red);
            spriteBatch.Draw(texture, left, null, Color.Red);
            spriteBatch.Draw(texture, right, null, Color.Red);
            
        }

        public void Jump(int moveSpeed)
        {
            speedY = -moveSpeed;
            canJump = false;
            jumpPoint = new Vector2(boundary.X, boundary.Y);
        }

        public virtual void Special()
        {

        }
    }
}
