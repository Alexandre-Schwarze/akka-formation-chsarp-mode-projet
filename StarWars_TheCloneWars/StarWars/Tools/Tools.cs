using System;
using System.Collections.Generic;
using System.Text;

namespace StarWars.Tools
{
    public enum ForceSide
    {
        Light = 0,
        Dark = 1
    }

    public class Position
    {
        char absciss;
        int ordinate;
    }
    
    public static class Tools
    {

        private static int txtoffset = 12;

        public static void RightOffsetWriteLine(string txt)
        {
            string offset = String.Empty;

            for(int i =0; i < txtoffset; i++ )
                offset += " "; 

            Console.WriteLine(offset + txt);
        }
    }
}
