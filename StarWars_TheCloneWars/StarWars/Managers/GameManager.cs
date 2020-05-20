﻿using StarWars.Entities.Implements;
using StarWars.Entities.Implements.Childs;
using StarWars.Entities.Interfaces;
using StarWars.Objects;
using StarWars.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

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
            game.Size = 26;
            //game.Size = GridManager.ChooseIndex();
            game.Troops = GenerateTroops(game.Size);
        }
         
        public static void PlayGame()
        {
            while (game.Troops.Count > 0 )
            {
                //Tools.Tools.RightOffsetWriteLine("\r\n################ TOUR N°"+game.Current_turn_number+" ##############");
                 game.Grid = GridManager.DisplayGrid(game.Getalltroops(), game.Size);

                //Console.WriteLine("################ TOUR DU JOUEUR ##############");
                DoPJTurn();

                //Console.WriteLine("################ TOURS PNJ ##############");
                DoPNJTurn(game.Troops);

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
            PJMove(Console.ReadKey(true)); 
        }

        private static void PJMove(ConsoleKeyInfo key)
        {
            Position desiredpos = GridManager.CheckPlayer(game.Troops, game.PJ.Position, key, game.Grid.Index);
            if (desiredpos != null)
                game.PJ.Position = desiredpos;
            else
            {
                Console.WriteLine("Mauvaise position ou combinaison, veuillez réitérer");
                PJMove(Console.ReadKey(true));
            }



        }

        private static void DoPNJTurn(List<IBaseTroop> troops)
        {
            foreach (IBaseTroop troop in troops)
            {
                if (troop.Remaining_HP > 0)
                {
                    string logtroop = troop.GetType().Name + " en " + troop.Position.Txtpos;

                    //Check attaques : si autre PNJ autour > attaquer
                    logtroop += " cherche une cible...";

                    IBaseTroop targetabletroop = GridManager.CheckAroundForTroop(troops, troop, game.Size);
                    if (targetabletroop != null)
                    {
                        if (game.PJ == targetabletroop)
                            game.PJ.color = ConsoleColor.Red;

                        MethodInfo attackmethod = troop.GetType().GetTypeInfo().DeclaredMethods.FirstOrDefault();
                        object[] parameters = new object[1];
                        parameters[0] = targetabletroop;
                        attackmethod.Invoke(troop, parameters);
                        continue;
                    }
                    else
                        logtroop += " mais n'en trouve aucune ...";

                    //Check blessures : si blessé > se soigne / répare
                    logtroop += " vérifie s'il est blessé... ";
                    if (troop.Remaining_HP < troop.MaxHP)
                    {
                        if (troop.GetType().BaseType == typeof(Synthetic))
                            (troop as Synthetic).AutoRepair();
                        else
                            (troop as Organic).Heal();
                        continue;
                    }
                    else
                        logtroop += "mais n'est pas blessé ... ";

                    //Check déplacement : Si ni attaque ni soin > déplacer 
                    logtroop += " regarde autour de lui ...";
                    Position potentialPosition = GridManager.CheckAroundForTroopMove(game.Getalltroops(), troop, game.Size);
                    if (potentialPosition != null)
                    {
                        logtroop += " et se déplace en " + potentialPosition.Txtpos;
                        troop.Position = potentialPosition;
                    }
                    else
                        logtroop += " il est encerclé ! ";

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
            Tools.Tools.RightOffsetWriteLine("Personnages jouables disponibles ...");

            int cpt = 1;
            foreach (Tuple<string,string> playable in EntitiesManager.Playablesstats)
            {
                Console.WriteLine(cpt + "-" + playable.Item1 + " : " + playable.Item2);
                cpt++;
            }

            Tools.Tools.RightOffsetWriteLine("Veuillez saisir le numéro du personnage choisi ...");

            var input = Console.ReadLine();
            int rslt;
            int.TryParse(input, out rslt);
            if (rslt < EntitiesManager.Playablesstats.Count+1 && rslt > 0)
            {             
                string name = EntitiesManager.Playablesstats[rslt-1].Item1;
                IBaseTroop selectedchar = (IBaseTroop)Activator.CreateInstance(EntitiesManager.Playables.Where((e) => e.Name.Equals(name)).FirstOrDefault());
                Console.WriteLine("Caractéristiques "+ selectedchar.GetType().Name + " : PV:" + (selectedchar as IBaseTroop).MaxHP + ", Vitesse:" + (selectedchar as IBaseTroop).Speed + ", attaques spéciales:");
                
                if(selectedchar.GetType().GetTypeInfo().DeclaredMethods.Count() > 0)
                    selectedchar.GetType().GetTypeInfo().DeclaredMethods.ToList().ForEach((e) => Console.WriteLine("-" + e.Name));

                selectedchar.color = ConsoleColor.Green;
                return selectedchar;
            }
            else
            {
                Console.WriteLine("La valeur saisie ne correspond pas...");
                ChooseCharacter();
            }
            return null;
        }

        #endregion
    }
}
