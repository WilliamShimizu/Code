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
        int width;
        int height;

        public Camera(Viewport view)
        {
            this.view = view;
            width = view.Width / 2;
            height = view.Height / 2;
        }

        public void Update(GameTime gameTime, Rectangle boundary)
        {
            center = new Vector2(boundary.X + (boundary.Width / 2) - width, boundary.Y + (boundary.Height / 2) - height);
            transform = Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0)) * Matrix.CreateScale(new Vector3(1, 1, 0));
        }

        public Vector2 getWorldCoord(Vector2 viewSpace)
        {
            Matrix m = Matrix.CreateTranslation(new Vector3(center.X, center.Y, 0)) * Matrix.CreateScale(new Vector3(1, 1, 0));
            return Vector2.Transform(viewSpace, m);
        }
    }
}
