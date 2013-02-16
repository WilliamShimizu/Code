using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LetsMakeAGame.Players
{
    class Musician : Player
    {
        bool specialIsActive;
        public override void Special()
        {
            base.Special();
            specialIsActive = true;
        }

        public void ReleaseSpecial()
        {
            specialIsActive = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (specialIsActive)
            {
                speedX *= 2;
                speedY *= 2;
            }
        }
    }
}
