using StarWars.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarWars.Entities.Implements
{
    public class ObiWan : IForceUser
    {
        public int HP { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Remaining_HP { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Speed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int ForceSide { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int ActionPoints { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int IsPlayable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int XP_value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int XP_total { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int LVL { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void BaseAttack()
        {
            throw new NotImplementedException();
        }

        public void BaseMove()
        {
            throw new NotImplementedException();
        }

        public void Bleed()
        {
            throw new NotImplementedException();
        }

        public void ForceGrab()
        {
            throw new NotImplementedException();
        }

        public void ForceJump()
        {
            throw new NotImplementedException();
        }

        public void ForcePush()
        {
            throw new NotImplementedException();
        }

        public void Heal()
        {
            throw new NotImplementedException();
        }
    }
}
