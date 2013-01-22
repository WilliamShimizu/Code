#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace LetsMakeAGame
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        private static Game_Main game;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            game = new Game_Main();
            game.Run();
        }
    }
}
