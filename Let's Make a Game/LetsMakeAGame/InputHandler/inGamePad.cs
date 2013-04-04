using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace InputHandler
{
    class inGamePad
    {
        private static GamePadState currentGamePadState;
        private static GamePadState previousGamePadState;

        public static List<InputManager.ACTIONS> GetInput()
        {
            currentGamePadState = GamePad.GetState(PlayerIndex.One);
            List<InputManager.ACTIONS> actions = new List<InputManager.ACTIONS>();

            if (currentGamePadState.IsButtonUp(Buttons.A) && previousGamePadState.IsButtonDown(Buttons.A))
            {
                actions.Add(InputManager.ACTIONS.JUMP);
            }

            if (currentGamePadState.ThumbSticks.Left.X > 0 || currentGamePadState.DPad.Right == ButtonState.Pressed)
            {
                actions.Add(InputManager.ACTIONS.RIGHT);
            }
            else if (currentGamePadState.ThumbSticks.Left.X < 0 || currentGamePadState.DPad.Left == ButtonState.Pressed)
            {
                actions.Add(InputManager.ACTIONS.LEFT);
            }

            if (currentGamePadState.IsButtonUp(Buttons.Y) && previousGamePadState.IsButtonDown(Buttons.Y))
            {
                actions.Add(InputManager.ACTIONS.TOGGLE);
            }

            previousGamePadState = currentGamePadState;
            return actions;
        }
    }
}
