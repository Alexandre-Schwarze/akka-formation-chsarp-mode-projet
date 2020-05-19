using System;
using System.Collections.Generic;
using System.Text;

namespace StarWars.Entities.Implements.Childs
{
    public class Yoda : ForceUser
    {
        public Yoda()
        {
            this.MaxHP = 14;
            this.Remaining_HP = 14;
            this.LVL = 1;
            this.Speed = 2;
            this.XP_total = 0;
            this.ActionPoints = 1;
            this.Forceside = Tools.ForceSide.Light;
            this.Icon = 'Y';
        }
    }
}
