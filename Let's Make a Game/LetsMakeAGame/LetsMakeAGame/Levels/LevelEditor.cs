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
        private Tile highlightTile;
        SpriteFont font;
        private Tile replacement;
        private int tileIndex;
        private bool highlightSelection;
        Menu menu;
        List<string> textureNames;
        Level testLevel;

        private int speedX;
        private int speedY;

        public LevelEditor()
        {
            
            menuTexture = Game1.contentMgr.Load<Texture2D>("crappyMenu");
            replacement = new Tile(Game1.contentMgr.Load<Texture2D>("Tiles/engineeringBlock"), new Vector2(0, 0));
            font = Game1.contentMgr.Load<SpriteFont>("myFont");
            selectedTexture = Game1.contentMgr.Load<Texture2D>("Tiles/selectedTile");
            blankTile = Game1.contentMgr.Load<Texture2D>("Tiles/blankTile");
            highlightSelection = true;
            textureNames = new List<string>();
            menu = new Menu(menuTexture);
            tiles = new List<Tile>();
            mousePos = new Vector2();
            highlightTile = new Tile(selectedTexture, new Vector2(0,0));
            int height = Game1.viewport.Height;
            int width = Game1.viewport.Width;
            int textureWidth = highlightTile.boundary.Width;
            for (int i = 0; i < height; i += 40)
            {
                for (int j = 0; j < width; j += 40)
                {
                    tiles.Add(new Tile(blankTile, new Vector2(j, i)));
                }
            }
        }

        private void updateHighlightTexture(Rectangle boundary)
        {
            highlightTile.boundary.X = boundary.X - 5;
            highlightTile.boundary.Y = boundary.Y - 5;
            highlightTile.boundary.Width = boundary.Width + 10;
            highlightTile.boundary.Height = boundary.Height + 10;
        }

        public void Update(GameTime gameTime)
        {
            //GetInput(Game1.currentKeyboardState, Game1.previousKeyboardState, Game1.currentMouseState, Game1.previousMouseState, gameTime);
            if (isWithin(mousePos, menu.position, menuTexture)) highlightSelection = false;
            else highlightSelection = true;
            if (testLevel != null)
            {
                testLevel.GetInput(Game1.currentKeyboardState, Game1.previousKeyboardState, Game1.currentMouseState, Game1.previousMouseState, gameTime);
                testLevel.Update(gameTime);
            }
            else
            {
                {
                    for (int i = 0; i < tiles.Count; i++)
                    {
                        if (isWithin(mousePos, tiles[i].boundary))
                        {
                            updateHighlightTexture(tiles[i].boundary);
                            tileIndex = i;
                            replacement.boundary.X = tiles[i].boundary.X;
                            replacement.boundary.Y = tiles[i].boundary.Y;
                        }
                        tiles[i].Update(speedX, speedY);
                    }
                }
            }
            for (int i = 0; i < menu.tiles.Count; i++)
            {
                if (isWithin(mousePos, menu.tiles[i].boundary))
                {
                    updateHighlightTexture(menu.tiles[i].boundary);
                    highlightSelection = true;
                }
            }
            for (int i = 0; i < menu.buttons.Count; i++)
            {
                if (isWithin(mousePos, menu.buttons[i].boundary))
                {
                    highlightTile.boundary = menu.buttons[i].boundary;
                    updateHighlightTexture(menu.buttons[i].boundary);
                    highlightSelection = true;
                }
            }
            menu.Update(gameTime, speedX, speedY);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (testLevel != null)
            {
                testLevel.Draw(spriteBatch);
            }
            else
            {
                foreach (Tile t in tiles) spriteBatch.Draw(t.texture, t.boundary, null, Color.White);
                spriteBatch.DrawString(font, "mouse X: " + mousePos.X, new Vector2(0, 0), Color.Gray);
                spriteBatch.DrawString(font, "mouse Y: " + mousePos.Y, new Vector2(0, 20), Color.Gray);
                spriteBatch.DrawString(font, "tilePos X: " + highlightTile.boundary.X, new Vector2(0, 40), Color.Gray);
                spriteBatch.DrawString(font, "tilePos Y: " + highlightTile.boundary.Y, new Vector2(0, 60), Color.Gray);
                if (highlightSelection)
                {
                    spriteBatch.Draw(replacement.texture, replacement.boundary, null, Color.White);
                    spriteBatch.Draw(highlightTile.texture, highlightTile.boundary, null, Color.White);
                }
                //foreach (gridLine g in gridLines)
                //{
                //    g.Draw(spriteBatch);
                //}
            }
            menu.Draw(spriteBatch);
            if (highlightSelection && isWithin(mousePos, menu.position, menuTexture))
            {
                spriteBatch.Draw(highlightTile.texture, highlightTile.boundary, null, Color.White);
            }
        }

        private void Save()
        {
            HashSet<string> set = new HashSet<string>(textureNames);
            StreamWriter sw = new StreamWriter("Content/Maps/User Created Content/test.txt");
            sw.Write("!");
            foreach (string name in set)
            {
                sw.Write(" " + name);
            }
            sw.WriteLine();
            
            for (int i = 0; i < tiles.Count; i++)
            {
                if (i % 40 == 0) sw.WriteLine();
                string textureName = tiles[i].texture.Name.ToString();
                if (textureName == "Tiles/blankTile") sw.Write(" ");
                else
                {
                    string number = Game1.textureLookupTable[textureName.Replace("Tiles/","")].ToString();
                    sw.Write(number);
                }
            }
            sw.Close();
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
            if (testLevel != null)
            {
                if (currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
                {
                    if (isWithin(mousePos, menu.position, menuTexture))
                    {
                        bool isOverButton = false;
                        foreach (Button b in menu.buttons)
                        {
                            if (isWithin(mousePos, b.boundary))
                            {
                                switch (b.texture.Name.Replace("Buttons/", ""))
                                {
                                    case "playMapButton":
                                        //Do Stuff
                                        break;
                                    default:
                                        break;
                                }
                                isOverButton = true;
                                break;
                            }
                        }
                        if (!isOverButton)
                        {
                            if (menu.isHidden) menu.isHidden = false;
                            else menu.isHidden = true;
                        }
                    }
                }
                return;
            }
            //Mouse
            if (currentMouseState.LeftButton == ButtonState.Pressed)
            {
                if (!isWithin(mousePos, menu.position, menu.background) && tiles[tileIndex].texture.Name != replacement.texture.Name)
                {
                    tiles[tileIndex] = replacement.Copy(tiles[tileIndex].position);
                    string name = tiles[tileIndex].texture.Name.ToString();
                    if (name != "Tiles/blankTile") textureNames.Add(name.Replace("Tiles/",""));
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
                        break;
                    }
                }
                for (int i = 0; i < menu.buttons.Count; i++)
                {
                    if (isWithin(mousePos, menu.buttons[i].boundary))
                    {
                        switch (menu.buttons[i].texture.Name.Replace("Buttons/",""))
                        {
                            case "playMapButton":
                                tiles.Clear();
                                testLevel = new Level("null", "null", "Content/Maps/User Created Content/test.txt", null, null);
                                break;
                            case "saveMapButton":
                                Save();
                                break;
                            default:
                                break;
                        }
                        isOverButton = true;
                        break;
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
                string name = tiles[tileIndex].texture.Name.ToString();
                if (name != "Tiles/blankTile")
                {
                    //replacement = new Tile(blankTile, tiles[tileIndex].position);
                    textureNames.Remove(name);
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
                    if (tiles[i].texture.Name != "Tiles/blankTile") tiles[i] = new Tile(blankTile, tiles[i].position);
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
