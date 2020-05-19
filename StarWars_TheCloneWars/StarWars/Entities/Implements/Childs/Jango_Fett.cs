using StarWars.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarWars.Entities.Implements.Childs
{
    /// <summary>
    /// Classe de personnage Jango_Fett
    /// </summary>
    public class Jango_Fett : Organic
    {
      public Jango_Fett()
        {
            this.MaxHP = 18;
            this.Remaining_HP = 18;
            this.LVL = 1;
            this.Speed = 2;
            this.XP_total = 0;
            this.ActionPoints = 1;
            this.Forceside = Tools.ForceSide.Dark;
            this.Icon = 'J';
        }
    }
}
