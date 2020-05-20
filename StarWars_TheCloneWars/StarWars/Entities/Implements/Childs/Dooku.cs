using StarWars.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarWars.Entities.Implements.Childs
{
    /// <summary>
    /// Classe de personnage Dooku
    /// </summary>
    public class Dooku : ForceUser
    {
       public Dooku ()
        {
            this.MaxHP = 15;
            this.Remaining_HP = 15;
            this.LVL = 1;
            this.Speed = 2;
            this.XP_total = 0;
            this.ActionPoints = 1;
            this.Forceside = Tools.ForceSide.Dark;
            this.Icon = 'D';
        }

        public void MakashiiStrike(IBaseTroop target)
        {
            string log = String.Empty;
            log += "Dooku tranche " + target.GetType().Name + " en " + target.Position.Txtpos + " et lui inflige 12 points de dégats ! (PV " + target.GetType().Name + " restants : " + target.MaxHP + ")";
            target.Remaining_HP -= 12;
        }
    }
}
