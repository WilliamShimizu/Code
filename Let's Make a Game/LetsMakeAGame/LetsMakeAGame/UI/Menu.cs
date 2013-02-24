﻿using System;
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
        public List<Button> buttons;

        public Menu(Texture2D background)
        {
            this.background = background;
            position.X = Game1.viewport.Width - 20;
            position.Y = 0;
            isHidden = true;
            tiles = new List<Tile>();
            tiles.Add(new Tile(Game1.getTexture("Tiles/engineeringBlock"), new Vector2(1,1)));
            tiles.Add(new Tile(Game1.getTexture("Tiles/rockTile"), new Vector2(1, 1)));
            tiles.Add(new Tile(Game1.getTexture("Tiles/leopardTile"), new Vector2(1, 1)));
            tiles.Add(new Tile(Game1.getTexture("Tiles/starsTile"), new Vector2(1, 1)));
            tiles.Add(new Tile(Game1.getTexture("Tiles/spikes"), new Vector2(1, 1)));
            tiles.Add(new Tile(Game1.getTexture("Tiles/tiles"), new Vector2(1, 1)));
            tiles.Add(new Tile(Game1.getTexture("Tiles/Block"), new Vector2(1, 1)));
            buttons = new List<Button>();
            buttons.Add(new Button(Game1.getTexture("Buttons/playMapButton")));
            buttons[0].boundary.Y = background.Height + (int)position.Y - 10 - buttons[0].texture.Height;
            buttons[0].boundary.X = (int)position.X + 20;
            buttons.Add(new Button(Game1.getTexture("Buttons/saveMapButton")));
            buttons[1].boundary.Y = buttons[0].boundary.Y;
            buttons[1].boundary.X = (int)buttons[0].boundary.X + 20;
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
            buttons[0].boundary.Y = background.Height + (int)position.Y - 10 - buttons[0].texture.Height;
            buttons[0].boundary.X = (int)position.X + 20;
            buttons[1].boundary.Y = buttons[0].boundary.Y;
            buttons[1].boundary.X = (int)buttons[0].boundary.X + buttons[0].texture.Width + 20;
            foreach (Button b in buttons)
            {
                b.Update(gameTime, speedX, speedY);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, position, Color.White);
            foreach (Tile t in tiles)
            {
                t.Draw(spriteBatch);
            }
            foreach (Button b in buttons)
            {
                b.Draw(spriteBatch);
            }
        }
    }
}

public class Button
{
    public Texture2D texture;
    public Rectangle boundary;
    public string text;

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
