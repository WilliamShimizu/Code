using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections;

namespace eContentManager
{
    public class eContentManager
    {
        private ContentManager contentMgr;
        public Hashtable textureTable { get; set; }

        public eContentManager(ContentManager contentMgr)
        {
            this.contentMgr = contentMgr;
            textureTable = new Hashtable();
        }

        public void loadContent(string[] fileNames)
        {

        }

        public Texture2D getTexture(string fileName)
        {
            Texture2D texture;
            if (textureTable[fileName] == null)
            {
                texture = contentMgr.Load<Texture2D>(fileName);
                textureTable.Add(fileName, texture);
            }
            else texture = (Texture2D)textureTable[fileName];
            return texture;
        }
    }
}
