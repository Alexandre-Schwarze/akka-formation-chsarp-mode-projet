using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarWars.Managers
{
    public static class GameManager
    {
        #region Methods
        public static void Menu_NewGame()
        {
            Tools.Tools.RightOffsetWriteLine("Showing Character Selection Screen ...");
            ChooseCharacter();
            Tools.Tools.RightOffsetWriteLine("Showing Character Stats Screen ...");
            Tools.Tools.RightOffsetWriteLine("Starting game ...");
        }

        public static void ChooseCharacter()
        {
            Tools.Tools.RightOffsetWriteLine("Veuillez choisir votre personnage pour commencer ...");
            int i = 1;
            EntitiesManager.Playables.ForEach((e) =>
                {
                    Tools.Tools.RightOffsetWriteLine("\n"+i.ToString() + ". " + e.Name);
                    i++;
                });
           // Tools.Tools.troops.ForEach((e) => Tools.Tools.RightOffsetWriteLine("\n" + e.Name ));
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
