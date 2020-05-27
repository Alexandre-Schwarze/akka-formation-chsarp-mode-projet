using StarWars.Entities.Implements;
using StarWars.Entities.Implements.Childs;
using StarWars.Entities.Interfaces;
using StarWars.Objects;
using StarWars.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StarWars.Managers
{
    public sealed class GameManager
    {
        #region Attributs
        private static GameManager _instance;
        static readonly object instanceLock = new object();
        public Game game;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null) //Les locks prennent du temps, il est préférable de vérifier d'abord la nullité de l'instance.
                {
                    lock (instanceLock)
                    {
                        if (_instance == null) //on vérifie encore, au cas où l'instance aurait été créée entretemps.
                            _instance = new GameManager();
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region Ctor
        private GameManager()
        { }
        #endregion

        #region Methods

        public void NewGame()
        {
            Console.Clear();
            game = new Game();
            game.Size = GridManager.Instance.ChooseIndex();
            SetGame();
            PlayGame();
            EndGame();
        }
        public void SetGame()
        {
            while (game.PJ == null)
                game.PJ =  ChooseCharacter();

            game.Troops = GenerateTroops(game.Size);
            game.Grid = GridManager.Instance.DisplayGrid(game.Troops, game.PJ, game.Size);
        }      
        public  void PlayGame()
        {
       
            while (game.PJ != null)
            {
                //CustomConsole.RightOffsetWriteLine("\r\n################ TOUR N°"+game.Current_turn_number+" ##############");

                //CustomConsole.RightOffsetWriteLine("################ TOUR DU JOUEUR ##############");
                DoPJTurn();
                
                //CustomConsole.RightOffsetWriteLine("################ TOURS PNJ ##############");
                DoPNJTurn(game.Troops);

                EndTurn();
            }
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            CustomConsole.RightOffsetWriteLine("########## GAmE oVER ##########");
        }
        private  void EndGame()
        {
            System.Environment.Exit(1);
            game.Grid = null;
            game = null;
            MainManager.Instance.Welcome();
        }
        private  void EndTurn()
        {
            game.Troops = game.Troops.Where((e) => e.Remaining_HP > 0).ToList();         
            game.Current_turn_number++;
            if (game.PJ != null)
            {         
                if (game.PJ.Remaining_HP <= 0)
                    game.PJ = null;
                else
                    BDDManager.Instance.SaveGame(this.game);
            }
            game.Grid = GridManager.Instance.DisplayGrid(game.Troops, game.PJ, game.Size);
        }
        private  void DoPJTurn()
        {

            Console.WriteLine("A = Attaquer | \u2190 \u2191 \u2192 \u2193  = Déplacer | (spacebar) Attendre");
            ConsoleKeyInfo enteredkey = Console.ReadKey(true);

            ConsoleKey[] acceptables = new ConsoleKey[] {ConsoleKey.A, ConsoleKey.LeftArrow, ConsoleKey.UpArrow , ConsoleKey.DownArrow, ConsoleKey.RightArrow, ConsoleKey.Spacebar }; 

            while (!acceptables.Contains(enteredkey.Key))
                enteredkey = Console.ReadKey(true);

            if (enteredkey.Key == ConsoleKey.A)
                PJAttack();
            else if (enteredkey.Key == ConsoleKey.Spacebar)
                return;
            else
                PJMove(enteredkey); 
        }
        private  void PJAttack()
        {
            CustomConsole.ClearLastConsoleLine();
            IBaseTroop targetabletroop = GridManager.Instance.CheckAroundForTroop(game.Getalltroops(), game.PJ, game.Size);
            if (targetabletroop != null)
            {
                MethodInfo attackmethod = game.PJ.GetType().GetTypeInfo().DeclaredMethods.FirstOrDefault();
                object[] parameters = new object[1];
                parameters[0] = targetabletroop;
                attackmethod.Invoke(game.PJ, parameters);
                if (targetabletroop.Remaining_HP <= 0)
                    this.game.PNJKilledByPlayer++;
            }
            else 
            {
                CustomConsole.ClearLastConsoleLine();
                Console.WriteLine("Aucune cible à portée !");
                DoPJTurn();
            } 
        }
        private  void PJMove(ConsoleKeyInfo key)
        { 
            Position desiredpos = GridManager.Instance.CheckPlayer(game.Troops, game.PJ.Position, key, game.Grid.Index);
            if (desiredpos != null)
                game.PJ.Position = desiredpos;
            else
            {
                CustomConsole.ClearLastConsoleLine();
                Console.WriteLine("Mauvaise position ou combinaison, veuillez réitérer");
                PJMove(Console.ReadKey(true));
            }
        }
        private  void DoPNJTurn(List<IBaseTroop> troops)
        {
            foreach (IBaseTroop troop in troops)
            {
                if (troop.Remaining_HP > 0)
                {
                    string logtroop = troop.GetType().Name + " en " + troop.Position.Txtpos;

                    //Check attaques : si autre PNJ autour > attaquer
                    logtroop += " cherche une cible...";

                    IBaseTroop targetabletroop = GridManager.Instance.CheckAroundForTroop(game.Getalltroops(), troop, game.Size);
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
                    Position potentialPosition = GridManager.Instance.CheckAroundForTroopMove(game.Getalltroops(), troop, game.Size);
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
        private  List<IBaseTroop> GenerateTroops(int index)
        {
            CustomConsole.RightOffsetWriteLine("Generating troops ...");
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
        private  IBaseTroop ChooseCharacter()
        {
            CustomConsole.RightOffsetWriteLine("Personnages jouables disponibles ...");

            int cpt = 1;
            foreach (Tuple<string,string> playable in EntitiesManager.Playablesstats)
            {
                CustomConsole.RightOffsetWriteLine(cpt + "-" + playable.Item1 + " : " + playable.Item2);
                cpt++;
            }

            CustomConsole.RightOffsetWriteLine("Veuillez saisir le numéro du personnage choisi ...");

            var input = Console.ReadLine();
            int rslt;
            int.TryParse(input, out rslt);
            if (rslt < EntitiesManager.Playablesstats.Count+1 && rslt > 0)
            {             
                string name = EntitiesManager.Playablesstats[rslt-1].Item1;
                IBaseTroop selectedchar = (IBaseTroop)Activator.CreateInstance(EntitiesManager.Playables.Where((e) => e.Name.Equals(name)).FirstOrDefault());
                CustomConsole.RightOffsetWriteLine("Caractéristiques "+ selectedchar.GetType().Name + " : PV:" + (selectedchar as IBaseTroop).MaxHP + ", Vitesse:" + (selectedchar as IBaseTroop).Speed + ", attaques spéciales:");
                
                if(selectedchar.GetType().GetTypeInfo().DeclaredMethods.Count() > 0)
                    selectedchar.GetType().GetTypeInfo().DeclaredMethods.ToList().ForEach((e) => CustomConsole.RightOffsetWriteLine("-" + e.Name));

                selectedchar.color = ConsoleColor.Green;
                return selectedchar;
            }
            else
            {
                CustomConsole.RightOffsetWriteLine("La valeur saisie ne correspond pas...");
                return null;
            }
        }
        #endregion
    }
}
