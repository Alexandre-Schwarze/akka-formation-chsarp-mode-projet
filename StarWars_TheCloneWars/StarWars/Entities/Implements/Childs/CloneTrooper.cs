using StarWars.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarWars.Entities.Implements.Childs
{
    public class CloneTrooper : BaseCloneTrooper
    {
        public CloneTrooper()
        {
            this.Icon = '8';
            this.HP = 5;
            this.Remaining_HP = 5;
            this.LVL = 1;
            this.Forceside = Tools.ForceSide.Light;
            this.ActionPoints = 1;
            this.Speed = 2;
        }
    }
}
