using System;
using System.Collections.Generic;
using System.Text;

namespace StarWars.Entities.Interfaces
{
    interface ISynthetic : IBaseTroop
    {
        void MotherCoreSyncronizing();
        void AutoRepair();
        void Bug();
    }
}
