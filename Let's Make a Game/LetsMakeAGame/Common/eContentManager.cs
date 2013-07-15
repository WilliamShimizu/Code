using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections;

namespace Common
{
    public class eContentManager
    {
        private static ContentManager contentManager;
        public static Hashtable textureTable { get; set; }

        public static string contentDirectory { get; set; }

        private static eContentManager instance;

        private eContentManager(ContentManager contentMgr)
        {
            contentManager = contentMgr;
            textureTable = new Hashtable();
            contentDirectory = contentMgr.RootDirectory;
        }

        public static eContentManager getInstance(ContentManager contentMgr)
        {
            if (instance == null) instance = new eContentManager(contentMgr);
            return instance;
        }

        public static eContentManager getInstance()
        {
            return instance;
        }

        public void loadContent(string[] fileNames)
        {

        }

        public Texture2D getTexture(string fileName)
        {
            if (textureTable == null) textureTable = new Hashtable();
            Texture2D texture;
            if (textureTable[fileName] == null)
            {
                texture = contentManager.Load<Texture2D>(fileName);
                textureTable.Add(fileName, texture);
            }
            else texture = (Texture2D)textureTable[fileName];
            return texture;
        }

        public SpriteFont getSpriteFont(string fileName)
        {
            return contentManager.Load<SpriteFont>(fileName);
        }
    }
}
