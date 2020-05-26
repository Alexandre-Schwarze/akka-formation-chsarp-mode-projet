using System;
using System.Collections.Generic;
using System.Data;
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
            Console.Clear();
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
            CustomConsole.RightOffsetWriteLine("1. Nouvelle partie ");
            CustomConsole.RightOffsetWriteLine("2. Charger partie précédente ");
            CustomConsole.RightOffsetWriteLine("3. Quitter");
            CustomConsole.RightOffsetWriteLine("Veuillez saisir votre choix puis Entrée : ");
            
            switch (Console.ReadLine())
            {
                case "1":
                    CustomConsole.RightOffsetWriteLine("######### NOUVELLE PARTIE #########");
                    GameManager.Instance.NewGame();
                    break;
                case "2":
                    CustomConsole.RightOffsetWriteLine(">>> Chargement précédente partie ...");
                    Game game = BDDManager.Instance.GetSavedGame();
                    if (game.PJ != null)
                        LoadGame(game);
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Aucune donnée sauvegardée");
                        MainMenu();
                    }
                    break;
                case "3":
                    CustomConsole.RightOffsetWriteLine(">>> Vous nous quittez déjà ? à bientôt alors !");
                    System.Environment.Exit(1);
                    break;
                default:
                    CustomConsole.RightOffsetWriteLine(">>> Choix non reconnu");
                    Welcome();
                    break;
            }
        }

        public  void SearchSaveGame()
        {
            throw new NotImplementedException();
        }

        public void LoadGame(Game loadedgame)
        {
			GridManager.isTroopsInitialized = true;
            GameManager.Instance.game = loadedgame;
            loadedgame.Grid =  GridManager.Instance.DisplayGrid(loadedgame.Troops, loadedgame.PJ, loadedgame.Size);
            GameManager.Instance.PlayGame();
        }

        public  void DeleteSavedGame()
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}
