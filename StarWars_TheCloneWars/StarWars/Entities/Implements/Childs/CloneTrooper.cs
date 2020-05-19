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
            this.MaxHP = 5;
            this.Remaining_HP = 5;
            this.LVL = 1;
            this.Forceside = Tools.ForceSide.Light;
            this.ActionPoints = 1;
            this.Speed = 2;
        }

        #region Methods
        /// <summary>
        /// Attaque blaster 
        /// </summary>
        /// <param name="target">Cible de l'attaque</param>
        public void ShootBlasterRifle(IBaseTroop target)
        {
            int range = 20;
            Console.WriteLine("CloneTrooper utilise son blaster sur " + target.GetType().Name + " en " + target.Position.Txtpos);
            Console.WriteLine("Et lui inflige 5 points de dégats ! (PV " + target.GetType().Name + " restants : " + target.Remaining_HP + ")");
            target.Remaining_HP -= 5;
            
        }
        #endregion
    }
}
