using StarWars.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarWars.Objects
{
    public class Game
    {
        #region Attributes
        public string Name { get; set; }
        public int Basetroop_number { get; set; }
        public IBaseTroop[] Troops{ get; set; }
        public int Max_turns { get; set; }
        public int Current_turn_number { get; set; }
        public IBaseTroop PJ { get; set; }
        #endregion       
        #region Ctor
        public Game()
        {
            Name = "DefaultName";
        }
        #endregion        
        #region Methods
        #endregion
    }
}
