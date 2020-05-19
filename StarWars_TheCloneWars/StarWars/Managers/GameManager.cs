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
            SetGame();
            PlayGame();
            EndGame();
        }

        private static void SetGame()
        {
            game = new Game();
            game.PJ = ChooseCharacter();

            game.Size = GridManager.ChooseIndex();
            game.Troops = GenerateTroops(game.Size);
        }
         
        public static void PlayGame()
        {
            while (game.Troops.Count > 0 )
            {
                Tools.Tools.RightOffsetWriteLine("\r\n################ TOUR N°"+game.Current_turn_number+" ##############");
                game.Grid = GridManager.DisplayGrid(game.Getalltroops(), game.Size);

                Console.WriteLine("################ TOUR DU JOUEUR ##############");
                /// Implémenter tour du joueur
                Console.ReadLine();

                Console.WriteLine("################ TOURS PNJ ##############");
                DoPNJTurn(game.Troops);

                Tools.Tools.RightOffsetWriteLine("\r\n################ FIN DU TOUR N°" + game.Current_turn_number + " ##############");
                EndTurn();
            }
        }

        private static void EndGame()
        {
            throw new NotImplementedException();
        }

        private static void EndTurn()
        {
            game.Troops = game.Troops.Where((e) => e.Remaining_HP > 0).ToList();         
            game.Current_turn_number++;
        }

        private static void DoPJTurn()
        {

        }

        private static void DoPNJTurn(List<IBaseTroop> troops)
        {
            foreach (IBaseTroop troop in troops)
            {
                if (troop.Remaining_HP > 0)
                {
                    string logtroop = troop.GetType().Name + " en " + troop.Position.Txtpos;

                    //Check attaques : si autre PNJ autour > attaquer
                    logtroop += " vérifie si une cible est à proximité ...";

                    IBaseTroop targetabletroop = GridManager.CheckAroundForTroop(troops, troop);
                    if (targetabletroop != null)
                    {
                        MethodInfo attackmethod = troop.GetType().GetTypeInfo().DeclaredMethods.FirstOrDefault();
                        object[] parameters = new object[1];
                        parameters[0] = targetabletroop;
                        attackmethod.Invoke(troop, parameters);
                        continue;
                    }
                    else
                        logtroop += " mais n'entrouve aucune ...";

                    logtroop += " vérifie s'il est blessé... ";
                    if (troop.Remaining_HP < troop.MaxHP)
                    {
                        if (troop.GetType() == typeof(Synthetic))
                            (troop as Synthetic).AutoRepair();
                        else
                            (troop as Organic).Heal();
                        continue;
                    }
                    else
                        logtroop += "mais n'est pas blessé ... ";
                    //Check déplacement : Si 7 cases adjacentes libres ? > déplacer 

                    Tools.Tools.DelayedWriteLine(logtroop);
                    continue;
                }
                else
                    continue;
            }
            
        }

        private static List<IBaseTroop> GenerateTroops(int index)
        {
            Console.WriteLine("Generating troops ...");
            List<IBaseTroop> listtroops = new List<IBaseTroop>();
            int sidenumbers = (int)Math.Round((decimal)((index / 2)), 0);
            int sergentnumber = (int)Math.Round((decimal)(sidenumbers / 5), 0);

            for (int i = 0; i < sidenumbers; i++)
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

        /// <summary>
        /// Choix du personnage jouable
        /// </summary>
        /// <returns>Classe de PJ sélectionnée par l'utilisateur</returns>
        private static IBaseTroop ChooseCharacter()
        {
            object pj = new object();
            Tools.Tools.RightOffsetWriteLine("Personnages jouables disponibles ...");
            EntitiesManager.Playablesstats.ForEach((e) => Console.WriteLine("-" + e.Item1 + " : " + e.Item2));

            Tools.Tools.RightOffsetWriteLine("Veuillez saisir le nom du personnage choisi ...");

            var input = Console.ReadLine();
            if (EntitiesManager.Playablesstats.Select((e) => e.Item1).ToList().Contains(input))
            {
                pj = (IBaseTroop)Activator.CreateInstance(EntitiesManager.Playables.Where((e) => e.Name.Equals(input)).FirstOrDefault());
                Console.WriteLine("Caractéristiques personnage : PV:" + (pj as IBaseTroop).MaxHP + ", Vitesse:" + (pj as IBaseTroop).Speed + ", attaques spéciales:");
                pj.GetType().GetTypeInfo().DeclaredMethods.ToList().ForEach((e) => Console.WriteLine("-" + e.Name));
            }
            else
            {
                Console.WriteLine("La valeur saisie ne correspond pas...");
                ChooseCharacter();
            }

            return (IBaseTroop)pj;

        }


        #endregion
    }
}
