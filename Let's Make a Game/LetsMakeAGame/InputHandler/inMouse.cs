using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace InputHandler
{
    class inMouse
    {
        public static Vector2 cursorPos;
        private static MouseState currentMouseState;
        private static MouseState previousMouseState;
        private static int scrollWheelVal;

        public static List<InputManager.ACTIONS> GetInput()
        {
            List<InputManager.ACTIONS> actions = new List<InputManager.ACTIONS>();
            currentMouseState = Mouse.GetState();
            cursorPos.X = currentMouseState.X;
            cursorPos.Y = currentMouseState.Y;

            if (currentMouseState.LeftButton == ButtonState.Pressed)
            {
                actions.Add(InputManager.ACTIONS.LEFT_CLICK_DOWN);
            }
            else if (currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
            {
                actions.Add(InputManager.ACTIONS.LEFT_CLICK);
            }

            if (currentMouseState.RightButton == ButtonState.Pressed)
            {
                actions.Add(InputManager.ACTIONS.RIGHT_CLICK_DOWN);
            }
            else if (currentMouseState.RightButton == ButtonState.Released && previousMouseState.RightButton == ButtonState.Pressed)
            {
                actions.Add(InputManager.ACTIONS.RIGHT_CLICK);
            }

            scrollWheelVal = currentMouseState.ScrollWheelValue - previousMouseState.ScrollWheelValue;

            previousMouseState = currentMouseState;
            return actions;
        }

    }
}
