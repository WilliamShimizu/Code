using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using LetsMakeAGame.Players;
using LetsMakeAGame.UI;

namespace LetsMakeAGame
{
    public class LevelEditor
    {
        List<Tile> tiles;
        private Texture2D blankTile;
        private Texture2D selectedTexture;
        private Texture2D menuTexture;
        private Vector2 mousePos;
        private Tile selectedTilePos;
        SpriteFont font;
        private Tile replacement;
        private int tileIndex;
        private bool highlightSelection;
        Menu menu;

        private int speedX;
        private int speedY;

        public LevelEditor()
        {
            highlightSelection = true;
            menuTexture = Game1.contentMgr.Load<Texture2D>("crappyMenu");
            replacement = new Tile(Game1.contentMgr.Load<Texture2D>("engineeringBlock"), new Vector2(0, 0));
            font = Game1.contentMgr.Load<SpriteFont>("myFont");
            selectedTexture = Game1.contentMgr.Load<Texture2D>("selectedTile");
            blankTile = Game1.contentMgr.Load<Texture2D>("blankTile");
            menu = new Menu(menuTexture);
            tiles = new List<Tile>();
            mousePos = new Vector2();
            selectedTilePos = new Tile(selectedTexture, new Vector2(0,0));
            int height = Game1.viewport.Height;
            int width = Game1.viewport.Width;
            int textureWidth = selectedTilePos.boundary.Width;
            for (int i = 0; i < height; i += 40)
            {
                for (int j = 0; j < width; j += 40)
                {
                    tiles.Add(new Tile(blankTile, new Vector2(j, i)));
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            if (isWithin(mousePos, menu.position, menuTexture)) highlightSelection = false;
            else highlightSelection = true;
            for(int i = 0; i < tiles.Count; i++)
            {
                if (isWithin(mousePos, tiles[i].boundary))
                {
                    selectedTilePos.boundary.X = tiles[i].boundary.X - 5;
                    selectedTilePos.boundary.Y = tiles[i].boundary.Y - 5;
                    tileIndex = i;
                    replacement.boundary.X = tiles[i].boundary.X;
                    replacement.boundary.Y = tiles[i].boundary.Y;
                }
                tiles[i].Update(speedX, speedY);
            }
            for (int i = 0; i < menu.tiles.Count; i++)
            {
                if (isWithin(mousePos, menu.tiles[i].boundary))
                {
                    selectedTilePos.boundary.X = menu.tiles[i].boundary.X - 5;
                    selectedTilePos.boundary.Y = menu.tiles[i].boundary.Y - 5;
                    highlightSelection = true;
                }
            }
            menu.Update(gameTime, speedX, speedY);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile t in tiles) spriteBatch.Draw(t.texture, t.boundary, null, Color.White);
            spriteBatch.DrawString(font, "mouse X: " + mousePos.X, new Vector2(0, 0), Color.Gray);
            spriteBatch.DrawString(font, "mouse Y: " + mousePos.Y, new Vector2(0, 20), Color.Gray);
            spriteBatch.DrawString(font, "tilePos X: " + selectedTilePos.boundary.X, new Vector2(0, 40), Color.Gray);
            spriteBatch.DrawString(font, "tilePos Y: " + selectedTilePos.boundary.Y, new Vector2(0, 60), Color.Gray);
            if (highlightSelection) spriteBatch.Draw(selectedTilePos.texture, selectedTilePos.boundary, null, Color.White);
            menu.Draw(spriteBatch);
            if (highlightSelection && isWithin(mousePos, menu.position, menuTexture))
            {
                spriteBatch.Draw(selectedTilePos.texture, selectedTilePos.boundary, null, Color.White);
            }
            //foreach (gridLine g in gridLines)
            //{
            //    g.Draw(spriteBatch);
            //}
        }

        public void addObject()
        {

        }

        private bool isWithin(Vector2 mouse, Vector2 obj, Texture2D txtr)
        {
            return (mouse.X >= obj.X && mouse.X <= obj.X + txtr.Width && mouse.Y >= obj.Y && mouse.Y <= obj.Y + txtr.Height);
        }

        private bool isWithin(Vector2 mouse, Rectangle obj)
        {
            return (mouse.X >= obj.X && mouse.X <= obj.X + obj.Width && mouse.Y >= obj.Y && mouse.Y <= obj.Y + obj.Height);
        }

        public void GetInput(KeyboardState currentKeyboardState, KeyboardState previousKeyboardState, MouseState currentMouseState, MouseState previousMouseState, GameTime gameTime)
        {
            mousePos.X = currentMouseState.X;
            mousePos.Y = currentMouseState.Y;
            //Mouse
            if (currentMouseState.LeftButton == ButtonState.Pressed)
            {
                if (!isWithin(mousePos, menu.position, menu.background) && tiles[tileIndex].texture.Name != replacement.texture.Name)
                {
                    tiles[tileIndex] = replacement.Copy(tiles[tileIndex].position);
                    highlightSelection = false;
                }
            }

            if (currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
            {
                bool isOverButton = false;
                int tileIndex = 0;
                for(int i = 0; i < menu.tiles.Count; i++)
                {
                    if (isWithin(mousePos, menu.tiles[i].boundary))
                    {
                        tileIndex = i;
                        isOverButton = true;
                    }
                }
                if (isWithin(mousePos, menu.position, menu.background) && !isOverButton)
                {
                    if (menu.isHidden) menu.isHidden = false;
                    else menu.isHidden = true;
                }
                else if (isOverButton)
                {
                    replacement = menu.tiles[tileIndex].Copy();
                }
                highlightSelection = true;
            }

            if (currentMouseState.RightButton == ButtonState.Pressed)
            {
                if (tiles[tileIndex].texture.Name != "blankTile")
                {
                    tiles[tileIndex] = new Tile(blankTile, tiles[tileIndex].position);
                    highlightSelection = false;
                }
            }

            if (currentMouseState.RightButton == ButtonState.Released && previousMouseState.RightButton == ButtonState.Pressed)
            {
                highlightSelection = true;
            }

            //Keyboard
            if (currentKeyboardState.IsKeyDown(Keys.LeftShift) && currentKeyboardState.IsKeyDown(Keys.D9))
            {
                for (int i = 0; i < tiles.Count; i++)
                {
                    if (tiles[i].texture.Name != "blankTile") tiles[i] = new Tile(blankTile, tiles[i].position);
                }
            }

            if (currentKeyboardState.IsKeyDown(Keys.D)) speedX = 6;
            else if (currentKeyboardState.IsKeyDown(Keys.A)) speedX = -6;
            else speedX = 0;

            if (currentKeyboardState.IsKeyDown(Keys.W)) speedY = -6;
            else if (currentKeyboardState.IsKeyDown(Keys.S)) speedY = 6;
            else speedY = 0;

        }
    }
}
