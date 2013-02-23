using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LetsMakeAGame;

namespace LetsMakeAGame.UI
{
    class Menu
    {
        public Texture2D background { get; set; }
        public Vector2 position;
        public bool isHidden { get; set; }
        public List<Tile> tiles;

        public Menu(Texture2D background)
        {
            this.background = background;
            position.X = Game1.viewport.Width - 20;
            position.Y = 0;
            isHidden = true;
            tiles = new List<Tile>();
            tiles.Add(new Tile(Game1.getTexture("engineeringBlock"), new Vector2(1,1)));
            tiles.Add(new Tile(Game1.getTexture("rockTile"), new Vector2(1, 1)));
            tiles.Add(new Tile(Game1.getTexture("leopardTile"), new Vector2(1, 1)));
            tiles.Add(new Tile(Game1.getTexture("starsTile"), new Vector2(1, 1)));
            tiles.Add(new Tile(Game1.getTexture("spikes"), new Vector2(1, 1)));
            tiles.Add(new Tile(Game1.getTexture("tiles"), new Vector2(1, 1)));
            tiles.Add(new Tile(Game1.getTexture("Block"), new Vector2(1, 1)));
        }

        public void Update(GameTime gameTime, int speedX, int speedY)
        {
            if (isHidden) position.X = Game1.viewport.Width - 20;
            else position.X = Game1.viewport.Width - background.Width;
            position.X -= speedX;
            position.Y -= speedY;
            int x = (int)this.position.X + 10;
            int y = (int)this.position.Y + 10;
            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].boundary.X = x + ((i % 6) * 80);
                tiles[i].boundary.Y = y + ((i / 6) * 80);
                tiles[i].Update(speedX, speedY);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, position, Color.White);
            foreach (Tile t in tiles)
            {
                t.Draw(spriteBatch);
            }
        }
    }
}

public class Button
{
    public Texture2D texture;
    public Rectangle boundary;

    public Button(Texture2D texture)
    {
        this.texture = texture;
        boundary = Game1.getRect(texture);
    }

    public Button(Vector2 position, Texture2D texture)
    {
        this.texture = texture;
        boundary = Game1.getRect(position, texture);
    }

    public void Update(GameTime gameTime, int speedX, int speedY)
    {
        boundary.X -= speedX;
        boundary.Y -= speedY;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, boundary, null, Color.White);
    }
}
