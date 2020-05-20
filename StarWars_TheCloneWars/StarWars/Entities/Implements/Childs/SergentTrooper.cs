using StarWars.Entities.Interfaces;
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
            this.MaxHP = 10;
            this.Remaining_HP = 10;
            this.LVL = 1;
            this.Forceside = Tools.ForceSide.Light;
            this.ActionPoints = 1;
            this.Speed = 2;
        }

        /// <summary>
        /// Attaque lance-roquettes
        /// </summary>
        /// <param name="target">Cible de l'attaque</param>
        public void ShootRocketLauncher(IBaseTroop target)
        {
            int range = 20;
            string log = String.Empty;
            log += "Serge nt Trooper en " + this.Position.Txtpos + " utilise son lance-roquette sur " + target.GetType().Name + " en " + target.Position.Txtpos + "Et lui inflige 10 points de dégats ! "; ;
            target.Remaining_HP -= 10;

            if (target.Remaining_HP > 0)
                log += "( PV " + target.GetType().Name + " restants: " + target.Remaining_HP + ") ";
        }
    }
}
