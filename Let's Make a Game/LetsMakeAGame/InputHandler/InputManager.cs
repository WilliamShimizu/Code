using System.Collections.Generic;
using Microsoft.Xna.Framework;

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
            SELECT,
            LEFT_CLICK_DOWN,
            RIGHT_CLICK_DOWN,
            LEFT_CLICK,
            RIGHT_CLICK
        }

        public static HashSet<ACTIONS> GetInput()
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
    }
}
