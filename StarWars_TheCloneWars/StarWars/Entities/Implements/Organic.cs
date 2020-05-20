using StarWars.Entities.Interfaces;
using StarWars.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarWars.Entities.Implements
{
    public class Organic : IBaseTroop 
    {
        public int MaxHP { get ; set; }

        private int _remaining_HP;
        public int Remaining_HP { 
            get 
            {
                return _remaining_HP;
            }
            set { _remaining_HP = value;

                if (_remaining_HP <= 0)
                    Console.WriteLine(this.GetType().Name + " en " + this.Position.Txtpos + " est décédé ...");
            }
        }
        public int Speed { get; set; }
        public int ForceSide { get; set; }
        public int ActionPoints { get; set; }
        public int IsPlayable { get; set; }
        public int XP_value { get; set; }
        public int XP_total { get; set; }
        public int LVL { get; set; }
        public ForceSide Forceside { get; set; }
        public Position Position { get; set; }
        public char Icon  { get; set; }
        public ConsoleColor color { get; set; }

        public void Heal() 
        {
            this.Remaining_HP += 2;
            Console.WriteLine(this.GetType().Name + " se soigne de 2 HP ! Total : " + this.Remaining_HP);
        }
        public void Bleed() { }

        public void BaseAttack()
        {
            throw new NotImplementedException();
        }

        public void BaseMove()
        {
            throw new NotImplementedException();
        }

        public void Checkdeath()
        {
            throw new NotImplementedException();
        }
    }
}
