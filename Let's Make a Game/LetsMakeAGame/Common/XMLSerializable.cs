using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Common
{
    public class XMLSerializable
    {
        public int positionX { get; set; }
        public int positionY { get; set; }
        public int height { get; set; }
        public int width { get; set; }
        public string textureName;
        public Entity.ENTITY type;

        public XMLSerializable(Rectangle rect, string textureName, Entity.ENTITY type)
        {
            positionX = rect.X;
            positionY = rect.Y;
            height = rect.Height;
            width = rect.Width;
            this.textureName = textureName;
            this.type = type;
        }

        public void writeNode(XmlWriter writer)
        {
            writer.WriteStartElement(type.ToString());
                writer.WriteElementString("positionX", positionX.ToString());
                writer.WriteElementString("positionY", positionY.ToString());
                writer.WriteElementString("height", height.ToString());
                writer.WriteElementString("width", width.ToString());
                writer.WriteElementString("textureName", textureName);
            writer.WriteEndElement();
        }
    }
}
