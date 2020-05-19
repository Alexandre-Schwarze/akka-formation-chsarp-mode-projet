using System;
using System.Collections.Generic;
using System.Text;

namespace StarWars.Entities.Implements.Childs
{
    public class SergentTrooper : CloneTrooper 
    {
        public SergentTrooper()
        {
            this.Icon = 'O';
            this.HP = 10;
            this.Remaining_HP = 10;
            this.LVL = 1;
            this.Forceside = Tools.ForceSide.Light;
            this.ActionPoints = 1;
            this.Speed = 2;
        }
    }
}
