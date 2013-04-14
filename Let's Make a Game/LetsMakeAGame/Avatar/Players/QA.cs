using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TileManager;

namespace Avatar.Players
{
    public class QA : Player
    {

        public QA(Microsoft.Xna.Framework.Rectangle boundary, Microsoft.Xna.Framework.Graphics.Texture2D texture)
            : base(boundary, texture, ENTITY.QA)
        {

        }

        public void Special(Vector2 pos, List<Tile> tiles)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                Rectangle r = tiles[i].boundary;
                int top = r.Y;
                int bottom = r.Y + r.Height;
                int left = r.X;
                int right = r.X + r.Width;
                if (pos.X <= right && pos.X >= left && pos.Y >= top && pos.Y <= bottom)
                {
                    tiles[i] = null;
                    tiles.Remove(tiles[i]);
                }
            }
        }
    }
}
