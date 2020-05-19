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
            this.MaxHP = 5;
            this.Remaining_HP = 5;
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
            Console.WriteLine("Droide B1 utilise son fusil laser sur " + target.GetType().Name + " en " + target.Position.Txtpos);
            target.Remaining_HP -= 5;
            Console.WriteLine("Et lui inflige 5 points de dégats ! (PV " + target.GetType().Name + " restants : " + target.Remaining_HP + ")");
        }
        #endregion




    }
}
