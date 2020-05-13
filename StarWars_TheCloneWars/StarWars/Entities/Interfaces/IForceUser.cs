using StarWars.Entities.Implements;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarWars.Entities.Interfaces
{
    interface IForceUser : IOrganic 
    {
        void ForcePush();
        void ForceGrab();
        void ForceJump();
    }
}
