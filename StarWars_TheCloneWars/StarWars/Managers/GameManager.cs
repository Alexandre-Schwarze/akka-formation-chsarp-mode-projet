using StarWars.Entities.Interfaces;
using StarWars.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
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
            Tools.Tools.RightOffsetWriteLine("Showing Character Selection Screen ...");
            ChooseCharacter();
            Tools.Tools.RightOffsetWriteLine("Starting game ...");
        }

        /// <summary>
        /// Choix du personnage jouable
        /// </summary>
        /// <returns>Classe de PJ sélectionnée par l'utilisateur</returns>
        public static void ChooseCharacter()
        {
            Tools.Tools.RightOffsetWriteLine("Personnages jouables disponibles ...");
            EntitiesManager.Playablesstats.ForEach((e) => Tools.Tools.RightOffsetWriteLine("\r\n"+ e.Item1 + " : " + e.Item2 ));

            Tools.Tools.RightOffsetWriteLine("AnVeuillez saisir le nom du personnage choisi ...");

            var input = Console.ReadLine();
            if (EntitiesManager.Playablesstats.Select((e) => e.Item1).ToList().Contains(input))
                game.PJ = (IBaseTroop)Activator.CreateInstance(EntitiesManager.Playables.Where((e) => e.Name.Equals(input)).FirstOrDefault());
            else
            {
                Tools.Tools.RightOffsetWriteLine("La valeur saisie ne correspond pas...");
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
