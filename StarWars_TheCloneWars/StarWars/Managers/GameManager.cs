using StarWars.Entities.Implements;
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
        public static void NewGame()
        {
            game = new Game();
            ChooseCharacter();
            int indexgrid = GridManager.ChooseIndex();
            game.Troops = GenerateTroops(indexgrid);

            List<IBaseTroop> TroopsToPlace = new List<IBaseTroop>(game.Troops);
            TroopsToPlace.Add(game.PJ);

            game.Grid = GridManager.DisplayGrid(TroopsToPlace, indexgrid);

            Console.WriteLine("Starting game ...");
            PlayGame(game);
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
        
        public static void PlayGame(Game game)
        {
            while (game.Current_turn_number < game.Max_turns )
            {
                Tools.Tools.RightOffsetWriteLine("\r\n################ TOUR N°"+game.Current_turn_number+" ##############");

                Tools.Tools.RightOffsetWriteLine("################ TOUR DU JOUEUR ##############");
                /// Implémenter tour du joueur
                Console.ReadLine();

                Tools.Tools.RightOffsetWriteLine("################ TOURS PNJ ##############");
                DoPNJTurn(game.Troops);

                game.Current_turn_number++;
            }
        }

        private static void DoPNJTurn(List<IBaseTroop> troops)
        {
            foreach (IBaseTroop troop in troops)
            {
                switch (troop.Forceside)
                {
                    case Tools.ForceSide.Dark:
                        //Check attaques : si autre PNJ autour > attaquer
                        //Check heal : si PV < PV max > heal/repair
                        if (troop.Remaining_HP < troop.HP)
                        {
                            if (troop.GetType().IsSubclassOf(typeof(Synthetic)))
                                (troop as Synthetic).AutoRepair();
                            else
                                (troop as Organic).Heal();
                            break;
                        }
                        //Check déplacement : Si 7 cases adjacentes libres ? > déplacer 


                        break;
                    case Tools.ForceSide.Light:
                        //Check déplacement : Si 7 cases adjacentes libres ? > déplacer 
                        //Check heal : si PV != PV max > heal/repair
                        Console.WriteLine(troop.GetType().Name + " vérifie s'il est blessé... ");
                        if (troop.Remaining_HP < troop.HP)
                        {
                            if (troop.GetType() == typeof(Synthetic))
                                (troop as Synthetic).AutoRepair();
                            else
                                (troop as Organic).Heal();
                            break;
                        }
                        else
                            Console.WriteLine(troop.GetType().Name + " n'est pas blessé ... ");
                        //Check attaques : si à range > attaque
                        break;
                }
            }
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
