using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace StarWars.Managers
{
    public class MenuManager
    {
        #region Attributes
        public String WelcomeText{ get; set; }
        public String CharacterChoicText { get; set; }
        public String GridSetUpText { get; set; }
        public String CharDataText { get; set; }

        public GameManager _GameManager { get; set; }
        #endregion

        #region Ctor
        public MenuManager()
        {
            Welcome();
            MainMenu();
        }
        #endregion

        #region Methods
        public void Welcome()
        {
            Console.WriteLine("###############################################################");
            Console.WriteLine("###             STAR WARS : L'ATTAQUE DES CLONES            ###");
            Console.WriteLine("###############################################################\n");
        }

        public void MainMenu()
        {
            Console.WriteLine("\n\n1. Nouvelle Partie ");
            Console.WriteLine("2. Charger partie sauvegardée (in progress)");
            Console.WriteLine("3. Quitter");
            Console.WriteLine("Veuillez saisir votre choix (1 ou 2) puis Entrée : ");
            
            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine(">>> new game");
                    break;
                case "2":
                    Console.WriteLine(">>> in progress");
                    MainMenu();
                    break;
                case "3":
                    Console.WriteLine(">>> Déjà ? à bientôt alors !\n");
                    System.Environment.Exit(1);
                    break;
                default:
                    Console.WriteLine(">>> Choix non reconnu\n");
                    MainMenu();
                    break;
            }
        }

        public void ChooseCharacter()
        {

        }

        public void SetUpGrid()
        {

        }

        public void CharacterStats()
        {

        }


        #endregion
    }
}
