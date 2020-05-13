using StarWars.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarWars.Entities.Interfaces 
{
    public interface IBaseTroop
    {
        #region Attributes
        int HP { get; set; }
        int Remaining_HP { get; set; }
        int Speed { get; set; }

       // CustomTypes.ForceSide side;

        int ActionPoints { get; set; }
        int Position { get; set; }
        int IsPlayable { get; set; }
        int XP_value { get; set; }
        int XP_total { get; set; }
        int LVL { get; set; }


        #endregion

        #region Ctor
        #endregion

        #region Methods
        void BaseAttack();
        void BaseMove();
        #endregion
    }
}
