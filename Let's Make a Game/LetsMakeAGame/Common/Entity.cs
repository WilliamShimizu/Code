using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class Entity : XMLSerializable
    {
        public Rectangle boundary;
        public Texture2D texture;

        public Entity(Rectangle boundary, string textureName, ENTITY type)
            : base(boundary, textureName, type)
        {
            this.boundary = boundary;
            this.textureName = textureName;
            this.type = type;
        }

        public Entity(Rectangle boundary, Texture2D texture, ENTITY type)
            : base(boundary, texture.Name.ToString(), type)
        {
            this.boundary = boundary;
            this.textureName = texture.Name.ToString();
            this.type = type;
            this.texture = texture;
        }
        public void Update(int speedX, int speedY)
        {
            boundary.X -= speedX;
            boundary.Y -= speedY;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, boundary, null, Color.White);
        }

        public enum ENTITY
        {
            Player,
            Designer,
            Engineer,
            Artist,
            Musician,
            QA,
            Enemy,
            UI,
            Tile,
            Background
        }

        //public Entity Copy()
        //{
        //    return new Entity(this.boundary, this.textureName, this.type);
        //}

    }
}
