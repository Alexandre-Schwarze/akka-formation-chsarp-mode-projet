using System;
using System.Collections.Generic;
using System.Text;

namespace StarWars.Managers
{
    public static class GameManager
    {
        #region Attributes
        public static GridManager _GridManager { get; set; }
        #endregion


        #region Methods
        public static void Menu_NewGame()
        {
            Tools.Tools.RightOffsetWriteLine("Showing Character Selection Screen ...");
            Tools.Tools.RightOffsetWriteLine("Showing Character Stats Screen ...");
            Tools.Tools.RightOffsetWriteLine("Starting game ...");
        }

        public static void ChooseCharacter()
        {

        }

        public static void SetUpGrid()
        {

        }

        public static void CharacterStats()
        {

        }
        #endregion
    }
}
