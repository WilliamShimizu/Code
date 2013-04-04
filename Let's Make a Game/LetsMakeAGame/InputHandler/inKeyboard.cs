using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace InputHandler
{
    class inKeyboard
    {
        private static KeyboardState previousKeyboardState;
        private static KeyboardState currentKeyboardState;

        public static List<InputManager.ACTIONS> GetInput()
        {
            currentKeyboardState = Keyboard.GetState();

            List<InputManager.ACTIONS> actions = new List<InputManager.ACTIONS>();

            if (currentKeyboardState.IsKeyDown(Keys.A))
            {
                actions.Add(InputManager.ACTIONS.LEFT);
            }
            else if (currentKeyboardState.IsKeyDown(Keys.D))
            {
                actions.Add(InputManager.ACTIONS.RIGHT);
            }

            if (currentKeyboardState.IsKeyDown(Keys.W))
            {
                actions.Add(InputManager.ACTIONS.UP);
            }
            else if (currentKeyboardState.IsKeyDown(Keys.S))
            {
                actions.Add(InputManager.ACTIONS.DOWN);
            }

            if (currentKeyboardState.IsKeyUp(Keys.Space) && previousKeyboardState.IsKeyDown(Keys.Space))
            {
                actions.Add(InputManager.ACTIONS.JUMP);
            }

            if (currentKeyboardState.IsKeyUp(Keys.T) && previousKeyboardState.IsKeyDown(Keys.T))
            {
                actions.Add(InputManager.ACTIONS.TOGGLE);
            }

            if (currentKeyboardState.IsKeyDown(Keys.G))
            {

            }

            previousKeyboardState = currentKeyboardState;
            return actions;
        }
    }
}
