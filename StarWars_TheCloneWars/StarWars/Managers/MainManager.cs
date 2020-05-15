using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using StarWars.Tools;

namespace StarWars.Managers
{
    public static class MainManager
    {
        #region Attributes

        #endregion

        #region Methods
        public static void Welcome()
        {
            Tools.Tools.RightOffsetWriteLine("###############################################################");
            Tools.Tools.RightOffsetWriteLine("###             STAR WARS : L'ATTAQUE DES CLONES            ###");
            Tools.Tools.RightOffsetWriteLine("###############################################################\n");
        }

        public static void MainMenu()
        {
            Tools.Tools.RightOffsetWriteLine("1. Nouvelle Partie ");
            Tools.Tools.RightOffsetWriteLine("2. Charger partie sauvegardée (in progress)");
            Tools.Tools.RightOffsetWriteLine("3. Quitter");
            Tools.Tools.RightOffsetWriteLine("Veuillez saisir votre choix (1 ou 2) puis Entrée : ");
            
            switch (Console.ReadLine())
            {
                case "1":
                    Tools.Tools.RightOffsetWriteLine(">>> new game");
                    GameManager.Menu_NewGame();
                    break;
                case "2":
                    Tools.Tools.RightOffsetWriteLine(">>> in progress");
                    MainMenu();
                    break;
                case "3":
                    Tools.Tools.RightOffsetWriteLine(">>> Vous nous quittez déjà ? à bientôt alors !\n");
                    System.Environment.Exit(1);
                    break;
                default:
                    Tools.Tools.RightOffsetWriteLine(">>> Choix non reconnu\n");
                    MainMenu();
                    break;
            }
        }

        public static void SearchSaveGame()
        {

        }

        public static void LoadGame()
        {

        }

        public static void SaveGame()
        {

        }

        public static void DeleteSavedGame()
        {

        }


        #endregion
    }
}
