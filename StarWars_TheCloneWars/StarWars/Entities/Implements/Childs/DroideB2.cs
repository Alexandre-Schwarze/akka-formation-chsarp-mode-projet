using StarWars.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarWars.Entities.Implements.Childs
{
    public class DroideB2 : Synthetic
    {
      public DroideB2 ()
        {
            this.Icon = '§';
            this.HP = 10;
            this.LVL = 1;
            this.Forceside = Tools.ForceSide.Dark;
            this.ActionPoints = 1;
            this.Speed = 2;
        }
    }
}
