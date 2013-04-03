using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace InputHandler
{
    public class InputManager
    {
        public static Vector2 cursorPosition;

        public enum ACTIONS
        {
            LEFT,
            RIGHT,
            UP,
            DOWN,
            JUMP,
            SPECIAL,
            TOGGLE,
            LEFT_CLICK_DOWN,
            RIGHT_CLICK_DOWN,
            LEFT_CLICK,
            RIGHT_CLICK
        }

        public HashSet<ACTIONS> GetInput()
        {
            cursorPosition = inMouse.cursorPos;
            HashSet<ACTIONS> actions = new HashSet<ACTIONS>();
            foreach (ACTIONS a in inKeyboard.GetInput())
            {
                actions.Add(a);
            }
            foreach (ACTIONS a in inMouse.GetInput())
            {
                actions.Add(a);
            }
            foreach (ACTIONS a in inGamePad.GetInput())
            {
                actions.Add(a);
            }
            foreach (ACTIONS a in inTouchPanel.GetInput())
            {
                actions.Add(a);
            }
            return actions;
        }

        public bool isWithin(Vector2 obj, Texture2D txtr)
        {
            return (cursorPosition.X >= obj.X && cursorPosition.X <= obj.X + txtr.Width && cursorPosition.Y >= obj.Y && cursorPosition.Y <= obj.Y + txtr.Height);
        }

        public bool isWithin(Rectangle obj)
        {
            return (cursorPosition.X >= obj.X && cursorPosition.X <= obj.X + obj.Width && cursorPosition.Y >= obj.Y && cursorPosition.Y <= obj.Y + obj.Height);
        }

    }
}
