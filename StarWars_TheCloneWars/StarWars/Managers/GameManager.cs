using StarWars.Entities.Interfaces;
using StarWars.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace StarWars.Managers
{
    public static class GameManager
    {
        private static Game game;
        #region Methods
        public static void Menu_NewGame()
        {
            game = new Game();
            ChooseCharacter();
            game.Grid = GridManager.GenereateGrid(GridManager.ChooseIndex());

            Tools.Tools.RightOffsetWriteLine("Starting game ...");
        }

        /// <summary>
        /// Choix du personnage jouable
        /// </summary>
        /// <returns>Classe de PJ sélectionnée par l'utilisateur</returns>
        public static void ChooseCharacter()
        {
            Tools.Tools.RightOffsetWriteLine("Personnages jouables disponibles ...");
            EntitiesManager.Playablesstats.ForEach((e) => Console.WriteLine("-"+e.Item1 + " : " + e.Item2 ));

            Tools.Tools.RightOffsetWriteLine("Veuillez saisir le nom du personnage choisi ...");

            var input = Console.ReadLine();
            if (EntitiesManager.Playablesstats.Select((e) => e.Item1).ToList().Contains(input))
            {
                game.PJ = (IBaseTroop)Activator.CreateInstance(EntitiesManager.Playables.Where((e) => e.Name.Equals(input)).FirstOrDefault());
                Console.WriteLine("Caractéristiques personnage : PV:" + game.PJ.HP + ", Vitesse:"+game.PJ.Speed+", attaques spéciales:" );
                game.PJ.GetType().GetTypeInfo().DeclaredMethods.ToList().ForEach((e) => Console.WriteLine("-"+e.Name));
            }
            else
            {
                Console.WriteLine("La valeur saisie ne correspond pas...");
                ChooseCharacter();
            }
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
