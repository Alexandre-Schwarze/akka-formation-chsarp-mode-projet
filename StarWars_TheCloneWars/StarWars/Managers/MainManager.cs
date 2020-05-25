using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using StarWars.Objects;
using StarWars.Tools;

namespace StarWars.Managers
{
    public sealed class MainManager
    {
        #region Attributs
        private static MainManager _instance;
        static readonly object instanceLock = new object();
        public static MainManager Instance
        {
            get
            {
                if (_instance == null) //Les locks prennent du temps, il est préférable de vérifier d'abord la nullité de l'instance.
                {
                    lock (instanceLock)
                    {
                        if (_instance == null) //on vérifie encore, au cas où l'instance aurait été créée entretemps.
                            _instance = new MainManager();
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region Ctor
        private MainManager() { }
        #endregion

        #region Methods
        public  void Welcome()
        {
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("                         _                                      ");
            Console.WriteLine("                     ___| |_ __ _ _ __  __      ____ _ _ __ ___ ");
            Console.WriteLine("                    / __| __/ _` | '__| \\ \\ /\\ / / _` | '__/ __|");
            Console.WriteLine("                    \\__ \\ || (_| | |     \\ V  V / (_| | |  \\__ \\");
            Console.WriteLine("                    |___/\\__\\__,_|_|      \\_/\\_/ \\__,_|_|  |___/");
            Console.WriteLine("__.-.__         ########################################################         __.-.__");
            Console.WriteLine("'-._.-'         ###             L'ATTAQUE DES CLONES                 ###         '-._.-'");
            Console.WriteLine("                ########################################################");

            MainMenu();
        }

        public  void MainMenu()
        {
            CustomConsole.RightOffsetWriteLine("1. Nouvelle Partie ");
            CustomConsole.RightOffsetWriteLine("2. Charger partie sauvegardée (in progress)");
            CustomConsole.RightOffsetWriteLine("3. Quitter");
            CustomConsole.RightOffsetWriteLine("Veuillez saisir votre choix (1 ou 2) puis Entrée : ");
            
            switch (Console.ReadLine())
            {
                case "1":
                    CustomConsole.RightOffsetWriteLine("######### NOUVELLE PARTIE #########");
                    GameManager.Instance.NewGame();
                    break;
                case "2":
                    CustomConsole.RightOffsetWriteLine(">>> in progress");
                    MainMenu();
                    break;
                case "3":
                    CustomConsole.RightOffsetWriteLine(">>> Vous nous quittez déjà ? à bientôt alors !");
                    System.Environment.Exit(1);
                    break;
                default:
                    CustomConsole.RightOffsetWriteLine(">>> Choix non reconnu");
                    MainMenu();
                    break;
            }
        }

        public  void SearchSaveGame()
        {
            throw new NotImplementedException();
        }

        public  void LoadGame(Game gametoload)
        {
            GameManager.Instance.game = gametoload;
            GameManager.Instance.PlayGame();
        }

        public  void SaveGame(Game gametosave)
        {
            throw new NotImplementedException();
        }

        public  void DeleteSavedGame()
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}
