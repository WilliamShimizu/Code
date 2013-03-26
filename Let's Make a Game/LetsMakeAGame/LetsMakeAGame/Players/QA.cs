using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LetsMakeAGame.Players
{
    class QA : Player
    {

        public QA(Rectangle boundary, Texture2D texture)
            : base(boundary, texture, ENTITY.QA)
        {

        }
        public void Special(Vector2 pos, Level level)
        {
            for (int i = 0; i < level.tiles.Count; i++)
            {
                Rectangle r = level.tiles[i].boundary;
                int top = r.Y;
                int bottom = r.Y + r.Height;
                int left = r.X;
                int right = r.X + r.Width;
                if (pos.X <= right && pos.X >= left && pos.Y >= top && pos.Y <= bottom)
                {
                    level.tiles[i] = null;
                    level.tiles.Remove(level.tiles[i]);
                }
            }
        }
    }
}
