using StarWars.Entities.Implements.Childs;
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
            int indexgrid = GridManager.ChooseIndex();
            game.Troops = GenerateTroops(indexgrid);
            game.Troops.Add(game.PJ);


            game.Grid = GridManager.DisplayGrid(game.Troops, indexgrid);


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

        public static List<IBaseTroop> GenerateTroops(int index)
        {
            Console.WriteLine("Generating troops ...");
            List<IBaseTroop> listtroops = new List<IBaseTroop>();
            int sidenumbers = (int)Math.Round((decimal)((index / 2) - 5), 0);
            int sergentnumber = (int)Math.Round((decimal)(sidenumbers / 5),0);

            for (int i = 0; i < sidenumbers; i ++)
            {
                listtroops.Add(new DroideB1());
                listtroops.Add(new CloneTrooper());
            }

            for (int i = 0; i < sergentnumber; i++)
            {
                listtroops.Add(new DroideB2());
                listtroops.Add(new SergentTrooper());
            }

            return listtroops;
        }
        #endregion
    }
}
