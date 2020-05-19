using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace StarWars.Tools
{
    /// <summary>
    /// Type personnalisé pour représenter le côté de la Force des participants
    /// </summary>
    public enum ForceSide
    {
        Light = 0,
        Dark = 1
    }

    /// <summary>
    /// Type personnalisé pour les positions au sein de la grille
    /// </summary>
    public class Position
    {
        public string absciss;
        public int ordinate;
    }

    /// <summary>
    /// Classe utilitaire 
    /// </summary>
    public static class Tools
    {
        #region Outillages Console
        [DllImport("kernel32.dll", ExactSpelling = true)]
        public static extern IntPtr GetConsoleWindow();
        public static IntPtr ThisConsole = GetConsoleWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        public const int HIDE = 0;
        public const int MAXIMIZE = 3;
        public const int MINIMIZE = 6;
        public const int RESTORE = 9;
        private static int txtoffset = 12;

        public static void RightOffsetWriteLine(string txt)
        {
            Console.WriteLine("\r\n");
            string offset = String.Empty;

            for (int i = 0; i < txtoffset; i++)
                offset += " ";

            Console.WriteLine(offset + txt);
        }

        public static void SetConsolePosition()
        {
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            ShowWindow(ThisConsole, MAXIMIZE);
        }
        #endregion

        #region Outillage Dossiers
        private static string UserAppFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static string GameFolder = UserAppFolderPath + "\\CloneWars";
        public static string SaveFolder = GameFolder + "\\Saves";
        public static string DataFolder = GameFolder + "\\Data";
        public static string PlayablesFile = DataFolder + "\\PJs.xml";

        public static void InitFolders()
        {
            System.IO.Directory.CreateDirectory(GameFolder);
            System.IO.Directory.CreateDirectory(SaveFolder);
            System.IO.Directory.CreateDirectory(DataFolder);

            using (StreamWriter sw = new StreamWriter(PlayablesFile, true))
                sw.Write(Properties.Resources.PJs);
        }
        #endregion

        #region Outillage Calcul de positions 
        public static char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        private static int GetAbscissIndex(string absciss)
        {
            return Array.IndexOf(alpha, absciss);
        }

        /// <summary>
        /// Calcule si une attaque a suffisamment de portée pour atteindre sa cible
        /// </summary>
        /// <param name="range">portée de l'attaque</param>
        /// <param name="target">Position cible</param>
        /// <param name="striker">Position attaquant</param>
        /// <returns>Cible à portée de l'attaque ou non</returns>
        public static bool IsRangeOK(int range, Position target, Position striker)
        {
            if ((Math.Abs(GetAbscissIndex(target.absciss) - GetAbscissIndex(striker.absciss)) <= range) || (Math.Abs(GetAbscissIndex(striker.absciss) - GetAbscissIndex(target.absciss)) <= range) &&
                (Math.Abs(target.ordinate - striker.ordinate) <= range) || (Math.Abs(striker.ordinate - target.ordinate) <= range))
                return true;
            else return false;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		public static int ConvertFromStringBase26(string val)
		{
			string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			int res = 0;

			if (val.Length == 2)
				res = 26 * (letters.IndexOf(val[0]) + 1) + letters.IndexOf(val[1]);
			else if (val.Length == 1)
				res = letters.IndexOf(val[0]);

			return res;
		}

		public static string ConvertToStringBase26(int index)
		{
			string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			string ordinate;

			if (index < 26)
				ordinate = letters[index].ToString() + ' ';
			else
			{
				int firstIndex = index / 26 - 1;
				int secondIndex = index - 26 * (int)(index / 26);
				ordinate = letters[firstIndex].ToString() + letters[secondIndex].ToString();
			}

			return ordinate;
		}
		#endregion
	}
}
