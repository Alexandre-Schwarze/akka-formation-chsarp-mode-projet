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
			this.color = ConsoleColor.Cyan;
		}

        /// <summary>
        /// Attaque lance-roquettes
        /// </summary>
        /// <param name="target">Cible de l'attaque</param>
        public void ShootRocketLauncher(IBaseTroop target)
        {
            int range = 20;
            Console.WriteLine(" utilise son lance-roquette sur " + target.GetType().Name + " en " + target.Position.Txtpos);
            Console.WriteLine("Et lui inflige 10 points de dégats ! (PV " + target.GetType().Name + " restants : " + target.Remaining_HP + ")");
            target.Remaining_HP -= 10;           
        }
    }
}
