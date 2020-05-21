using System;
using System.Collections.Generic;
using System.Text;

namespace StarWars.Tools
{

    /// <summary>
    /// Type personnalisé pour les positions au sein de la grille
    /// </summary>
    public class Position
    {
        public string Absciss;
        public int Ordinate;
        public string Txtpos => Absciss + Ordinate;
    }
}
