using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Common
{
    public class Camera
    {
        public Matrix transform;
        Viewport view;
        Vector2 center;

        public Camera(Viewport view)
        {
            this.view = view;
        }

        public void Update(GameTime gameTime, Rectangle boundary)
        {
            center = new Vector2(boundary.X + (boundary.Width / 2) - 800, boundary.Y + (boundary.Height / 2) - 450);
            transform = Matrix.CreateScale(new Vector3(1, 1, 0)) * Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0));
        }
    }
}
