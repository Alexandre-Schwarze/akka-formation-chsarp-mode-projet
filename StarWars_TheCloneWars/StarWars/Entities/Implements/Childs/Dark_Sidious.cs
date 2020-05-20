using StarWars.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarWars.Entities.Implements.Childs
{
    /// <summary>
    /// Classe de personnage jouable Dark Sidious
    /// </summary>
    public class Dark_Sidious : ForceUser
    {
       public Dark_Sidious()
        {
            this.MaxHP = 12;
            this.Remaining_HP = 12;
            this.LVL = 1;
            this.Speed = 2;
            this.XP_total = 0;
            this.ActionPoints = 1;
            this.Forceside = Tools.ForceSide.Dark;
            this.Icon = 'S';
        }

        public void LightningStrike(IBaseTroop target)
        {
            string log = String.Empty;
            log += "Dark_Sidious lance des éclairs sur " + target.GetType().Name + " en " + target.Position.Txtpos + " et lui inflige 15 points de dégats ! (PV " + target.GetType().Name + " restants : " + target.MaxHP + ")";
            target.Remaining_HP -= 15;
        }


    }
}
