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
using System.Xml;

using LetsMakeAGame.Players;
using LetsMakeAGame.UI;
using InputHandler;

namespace LetsMakeAGame
{
    public class LevelEditor
    {
        List<Tile> tiles;
        private Texture2D blankTile;
        private Texture2D selectedTexture;
        private Texture2D menuTexture;
        private Vector2 cursorPos;
        private Tile highlightTile;
        SpriteFont font;
        private Tile replacement;
        private int tileIndex;
        private bool highlightSelection;
        Menu menu;
        List<string> textureNames;
        Level testLevel;
        List<Tile> tempTiles;
        private Tile tempTile;

        private int speedX;
        private int speedY;

        public LevelEditor()
        {
            
            menuTexture = Game1.contentMgr.Load<Texture2D>("crappyMenu");
            replacement = new Tile(Game1.contentMgr.Load<Texture2D>("Tiles/engineeringBlock"), new Vector2(0, 0));
            font = Game1.contentMgr.Load<SpriteFont>("myFont");
            selectedTexture = Game1.contentMgr.Load<Texture2D>("Tiles/selectedTile");
            blankTile = Game1.contentMgr.Load<Texture2D>("Tiles/blankTile");
            tempTiles = new List<Tile>();
            highlightSelection = true;
            textureNames = new List<string>();
            menu = new Menu(menuTexture);
            tiles = new List<Tile>();
            cursorPos = new Vector2();
            highlightTile = new Tile(selectedTexture, new Vector2(0,0));
            int height = Game1.viewport.Height;
            int width = Game1.viewport.Width;
            int textureWidth = highlightTile.boundary.Width;
            for (int i = -height; i < height; i += 40)
            {
                for (int j = -width; j < width; j += 40)
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
            GetInput();
            if (isWithin(cursorPos, menu.position, menuTexture)) highlightSelection = false;
            else highlightSelection = true;
            if (testLevel != null)
            {
                testLevel.GetInput(Game1.currentKeyboardState, Game1.previousKeyboardState, Game1.currentMouseState, Game1.previousMouseState, gameTime);
                testLevel.Update(gameTime);
            }
            else
            {
                for (int i = 0; i < tiles.Count; i++)
                {
                    if (isWithin(cursorPos, tiles[i].boundary))
                    {
                        updateHighlightTexture(tiles[i].boundary);
                        tileIndex = i;
                        replacement.boundary.X = tiles[i].boundary.X;
                        replacement.boundary.Y = tiles[i].boundary.Y;
                    }
                    tiles[i].Update(speedX, speedY);
                }
            }
            //Lots of redundancy here. I'd like to try to combine these.
            for (int i = 0; i < menu.buttons.Count; i++)
            {
                if (isWithin(cursorPos, menu.buttons[i].boundary))
                {
                    updateHighlightTexture(menu.buttons[i].boundary);
                    highlightSelection = true;
                }
            }
            for (int i = 0; i < menu.tiles.Count; i++)
            {
                if (isWithin(cursorPos, menu.tiles[i].boundary))
                {
                    updateHighlightTexture(menu.tiles[i].boundary);
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
                spriteBatch.DrawString(font, "mouse X: " + cursorPos.X, new Vector2(0, 0), Color.Gray);
                spriteBatch.DrawString(font, "mouse Y: " + cursorPos.Y, new Vector2(0, 20), Color.Gray);
                spriteBatch.DrawString(font, "tilePos X: " + highlightTile.boundary.X, new Vector2(0, 40), Color.Gray);
                spriteBatch.DrawString(font, "tilePos Y: " + highlightTile.boundary.Y, new Vector2(0, 60), Color.Gray);
                if (highlightSelection)
                {
                    spriteBatch.Draw(replacement.texture, replacement.boundary, null, Color.White);
                    spriteBatch.Draw(highlightTile.texture, highlightTile.boundary, null, Color.White);
                }
            }
            menu.Draw(spriteBatch);
            if (highlightSelection && isWithin(cursorPos, menu.position, menuTexture))
            {
                spriteBatch.Draw(highlightTile.texture, highlightTile.boundary, null, Color.White);
            }
        }

        private void Save(bool temp)
        {
            HashSet<string> set = new HashSet<string>(textureNames);
            string saveName = temp ? "$temp" : "temp";
            StreamWriter sw = new StreamWriter("Content/Maps/User Created Content/" + saveName + ".txt");
            sw.Write("!");
            foreach (string name in set)
            {
                sw.Write(" " + name);
            }
            sw.WriteLine();
            
            for (int i = 0; i < tiles.Count; i++)
            {
                if (i % 80 == 0) sw.WriteLine();
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

        private void EditMap(string name)
        {
            foreach (Tile t in tempTiles)
            {
                tiles.Add(t);
            }
            testLevel = null;
            tempTiles.Clear();
        }



        private bool isWithin(Vector2 mouse, Vector2 obj, Texture2D txtr)
        {
            return (mouse.X >= obj.X && mouse.X <= obj.X + txtr.Width && mouse.Y >= obj.Y && mouse.Y <= obj.Y + txtr.Height);
        }

        private bool isWithin(Vector2 mouse, Rectangle obj)
        {
            return (mouse.X >= obj.X && mouse.X <= obj.X + obj.Width && mouse.Y >= obj.Y && mouse.Y <= obj.Y + obj.Height);
        }

        public void GetInput()
        {
            HashSet<InputManager.ACTIONS> actions = InputManager.GetInput();
            cursorPos = InputManager.cursorPosition;

            if (!(actions.Contains(InputManager.ACTIONS.LEFT) || actions.Contains(InputManager.ACTIONS.RIGHT))) speedX = 0;
            if (!(actions.Contains(InputManager.ACTIONS.UP) || actions.Contains(InputManager.ACTIONS.DOWN))) speedY = 0;
            foreach (InputManager.ACTIONS a in actions)
            {
                switch (a)
                {
                    case InputManager.ACTIONS.LEFT:
                        speedX = -6;
                        break;
                    case InputManager.ACTIONS.RIGHT:
                        speedX = 6;
                        break;
                    case InputManager.ACTIONS.UP:
                        speedY = -6;
                        break;
                    case InputManager.ACTIONS.DOWN:
                        speedY = 6;
                        break;
                    case InputManager.ACTIONS.JUMP:
                        break;
                    case InputManager.ACTIONS.SPECIAL:
                        break;
                    case InputManager.ACTIONS.SELECT:
                        break;
                    case InputManager.ACTIONS.LEFT_CLICK_DOWN:
                        replaceTiles();
                        break;
                    case InputManager.ACTIONS.RIGHT_CLICK_DOWN:
                        eraseTiles();
                        break;
                    case InputManager.ACTIONS.LEFT_CLICK:
                        clickMenuItem();
                        break;
                    case InputManager.ACTIONS.RIGHT_CLICK:
                        highlightSelection = true;
                        replacement = tempTile.Copy();
                        break;
                    default:
                        break;
                }
            }
        }

        private void eraseTiles()
        {
            string name = tiles[tileIndex].texture.Name.ToString();
            if (name != "Tiles/blankTile")
            {
                //replacement = new Tile(blankTile, tiles[tileIndex].position);
                if (replacement.texture.Name != "Tiles/blankTile") tempTile = replacement.Copy();
                replacement = new Tile(blankTile, tiles[tileIndex].position);
                textureNames.Remove(name);
                tiles[tileIndex] = new Tile(blankTile, tiles[tileIndex].position);
                highlightSelection = false;
            }
        }

        private void replaceTiles()
        {
            if (!isWithin(cursorPos, menu.position, menu.background) && tiles[tileIndex].texture.Name != replacement.texture.Name)
            {
                tiles[tileIndex] = replacement.Copy(tiles[tileIndex].position);
                string name = tiles[tileIndex].texture.Name.ToString();
                if (name != "Tiles/blankTile") textureNames.Add(name.Replace("Tiles/", ""));
                highlightSelection = false;
            }
        }

        private void clickMenuItem()
        {
            bool isOverButton = false;
            int tileIndex = 0;
            for (int i = 0; i < menu.tiles.Count; i++)
            {
                if (isWithin(cursorPos, menu.tiles[i].boundary))
                {
                    tileIndex = i;
                    isOverButton = true;
                    break;
                }
            }
            for (int i = 0; i < menu.buttons.Count; i++)
            {
                if (isWithin(cursorPos, menu.buttons[i].boundary))
                {
                    switch (menu.buttons[i].text)
                    {
                        case "Play Map":
                            foreach (Tile t in tiles)
                            {
                                tempTiles.Add(t);
                            }
                            Save(true);
                            tiles.Clear();
                            testLevel = new Level("null", "null", "Content/Maps/User Created Content/$temp.txt", null, null);
                            menu.buttons[i].text = "Edit Map";
                            break;
                        case "Save Map":
                            Save(false);
                            break;
                        default:
                            break;
                    }
                    isOverButton = true;
                    break;
                }
            }
            if (isWithin(cursorPos, menu.position, menu.background) && !isOverButton)
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

        public void GetInput(KeyboardState currentKeyboardState, KeyboardState previousKeyboardState, MouseState currentMouseState, MouseState previousMouseState, GameTime gameTime)
        {
            cursorPos.X = currentMouseState.X;
            cursorPos.Y = currentMouseState.Y;
            if (testLevel != null)
            {
                if (currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
                {
                    if (isWithin(cursorPos, menu.position, menuTexture))
                    {
                        bool isOverButton = false;
                        foreach (Button b in menu.buttons)
                        {
                            if (isWithin(cursorPos, b.boundary))
                            {
                                switch (b.text)
                                {
                                    case "Edit Map":
                                        EditMap("test");
                                        b.text = "Play Map";
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
                replaceTiles();
            }

            if (currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
            {
                clickMenuItem();
            }

            if (currentMouseState.RightButton == ButtonState.Pressed)
            {
                eraseTiles();
            }

            if (currentMouseState.RightButton == ButtonState.Released && previousMouseState.RightButton == ButtonState.Pressed)
            {
                highlightSelection = true;
                replacement = tempTile.Copy();
            }

            //Keyboard
            if (currentKeyboardState.IsKeyDown(Keys.LeftShift) && currentKeyboardState.IsKeyDown(Keys.D9))
            {
                for (int i = 0; i < tiles.Count; i++)
                {
                    if (tiles[i].texture.Name != "Tiles/blankTile") tiles[i] = new Tile(blankTile, tiles[i].position);
                }
            }

            if (currentKeyboardState.IsKeyDown(Keys.LeftShift) && currentKeyboardState.IsKeyDown(Keys.D8))
            {
                for (int i = 0; i < tiles.Count; i++)
                {
                    if (tiles[i].texture.Name == "Tiles/blankTile") tiles[i] = replacement.Copy(tiles[i].position);
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
