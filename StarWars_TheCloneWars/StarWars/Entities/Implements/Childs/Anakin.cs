using StarWars.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarWars.Entities.Implements.Childs
{
    public class Anakin : ForceUser
    {
        #region Attributes
        #endregion        
        #region Ctor
        public Anakin()
        {
            this.HP = 15;
            this.LVL = 1;
            this.Speed = 2;
            this.XP_total = 0;
            this.ActionPoints = 1;
            this.Forceside = Tools.ForceSide.Light;
        }
        #endregion        
        #region Methods
        public void DjemSoStrike(IBaseTroop target)
        {
            int range = 2;
            Console.WriteLine("Anakin bondit sur " + target.GetType().Name);
            if (Tools.Tools.IsRangeOK(range, target.Position, this.Position))
                target.HP -= 5;
            Console.WriteLine("Et lui inflige 5 points de dégats ! (PV "+ target.GetType().Name + " restants : " + target.HP);
        }
        #endregion
    }
}
