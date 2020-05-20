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
            this.MaxHP = 10;
            this.Remaining_HP = 10;
            this.LVL = 1;
            this.Forceside = Tools.ForceSide.Dark;
            this.ActionPoints = 1;
            this.Speed = 2;
			this.color = ConsoleColor.Yellow;
		}

        #region Methods
        /// <summary>
        /// Attaque fusil laser double 
        /// </summary>
        /// <param name="target">Cible de l'attaque</param>
        public void ShootLaserRifle(IBaseTroop target)
        {
            int range = 20;
            Console.WriteLine("Droide B2 utilise son double fusil laser sur " + target.GetType().Name + " en " + target.Position.Txtpos);
            Console.WriteLine("Et lui inflige 8 points de dégats ! (PV " + target.GetType().Name + " restants : " + target.Remaining_HP + ")");
            target.Remaining_HP -= 8;
            
        }
        #endregion
    }
}
