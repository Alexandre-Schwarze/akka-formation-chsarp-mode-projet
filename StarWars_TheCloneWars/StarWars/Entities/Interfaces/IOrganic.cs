﻿using StarWars.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarWars.Entities.Interfaces  
{
    interface IOrganic : IBaseTroop 
    {

        void Heal();
        void Bleed();
    }
}
