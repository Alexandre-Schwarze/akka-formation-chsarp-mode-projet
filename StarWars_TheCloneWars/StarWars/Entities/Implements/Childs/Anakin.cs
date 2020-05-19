using StarWars.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarWars.Entities.Implements.Childs
{
    /// <summary>
    /// Classe de personnage Anakin
    /// </summary>
    public class Anakin : ForceUser
    {
        #region Attributes
        #endregion        
        #region Ctor
        /// <summary>
        /// Initialisation Nouveau Anakin
        /// </summary>
        public Anakin()
        {
            this.MaxHP = 15;
            this.Remaining_HP = 15;
            this.LVL = 1;
            this.Speed = 2;
            this.XP_total = 0;
            this.ActionPoints = 1;
            this.Forceside = Tools.ForceSide.Light;
            this.Icon = 'A';
        }
        #endregion        
        #region Methods
        /// <summary>
        /// Attaque DjemSo 
        /// </summary>
        /// <param name="target">Cible de l'attaque</param>
        public void AttaqueDjemSo(IBaseTroop target)
        {
            int range = 2;
            Console.WriteLine("Anakin tente de bondir sur " + target.GetType().Name + " en " + target.Position.Txtpos);
            if (Tools.Tools.IsRangeOK(range, target.Position, this.Position))
            {
                Console.WriteLine("Et lui inflige 5 points de dégats ! (PV " + target.GetType().Name + " restants : " + target.MaxHP + ")");
                target.Remaining_HP -= 5;
            }
        }
        #endregion
    }
}
