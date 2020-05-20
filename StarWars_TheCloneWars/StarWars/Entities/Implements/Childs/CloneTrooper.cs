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
            this.MaxHP = 6;
            this.Remaining_HP = 6;
            this.LVL = 1;
            this.Forceside = Tools.ForceSide.Light;
            this.ActionPoints = 1;
            this.Speed = 2;
            this.color = ConsoleColor.Cyan;
        }

        #region Methods
        /// <summary>
        /// Attaque blaster 
        /// </summary>
        /// <param name="target">Cible de l'attaque</param>
        public void ShootBlasterRifle(IBaseTroop target)
        {
            int range = 20;
            string log = String.Empty;
            log +="CloneTrooper en "+ this.Position.Txtpos  +" utilise son blaster sur " + target.GetType().Name + " en " + target.Position.Txtpos;
            target.Remaining_HP -= 5;

            if (target.Remaining_HP > 0)
                log += "( PV " + target.GetType().Name + " restants: " + target.Remaining_HP + ") ";
        }
        #endregion
    }
}
