using StarWars.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarWars.Entities.Implements.Childs
{
    public class DroideB1 : Synthetic
    {
        public DroideB1()
        {
            this.Icon = '#';
            this.MaxHP = 6;
            this.Remaining_HP = 6;
            this.LVL = 1;
            this.Forceside = Tools.ForceSide.Dark;
            this.ActionPoints = 1;
            this.Speed = 2;
        }

        #region Methods
        /// <summary>
        /// Attaque fusil laser 
        /// </summary>
        /// <param name="target">Cible de l'attaque</param>
        public void ShootlaserRifle(IBaseTroop target)
        {
            int range = 20;
           

            string log = String.Empty;
            log += "Droide B1 utilise son fusil laser sur " + target.GetType().Name + " en " + target.Position.Txtpos + "Et lui inflige 5 points de dégats ! ";
            
            target.Remaining_HP -= 5;
           
            if(target.Remaining_HP > 0)
                log += "( PV " + target.GetType().Name + " restants: " + target.Remaining_HP + ") ";
        }
        #endregion




    }
}
