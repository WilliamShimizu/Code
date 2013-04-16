using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public abstract class Entity : XMLSerializable
    {
        public Rectangle boundary;
        public Texture2D texture;
        public Vector2 velocity;
        public Vector2 position;

        public Entity(Rectangle boundary, string fileName, ENTITY type)
            : base(boundary, fileName, type)
        {
            this.position.X = boundary.X;
            this.position.Y = boundary.Y;
            this.boundary = boundary;
            this.fileName = fileName;
            this.type = type;
        }

        public Entity(Rectangle boundary, Texture2D texture, ENTITY type)
            : base(boundary, texture.Name.ToString(), type)
        {
            this.position.X = boundary.X;
            this.position.Y = boundary.Y;
            this.boundary = boundary;
            this.fileName = texture.Name.ToString();
            this.type = type;
            this.texture = texture;
        }

        public virtual void Update()
        {
            position += velocity;
            boundary.X = (int)position.X;
            boundary.Y = (int)position.Y;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, boundary, Color.White);
            //spriteBatch.Draw(texture, boundary, null, Color.White);
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
            Background,
            Audio,
        }
    }
}
