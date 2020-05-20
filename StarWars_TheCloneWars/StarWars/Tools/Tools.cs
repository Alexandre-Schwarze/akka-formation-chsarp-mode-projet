using StarWars.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

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
        public string Absciss;
        public int Ordinate;
        public string Txtpos => Absciss + Ordinate;
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

        public static void DelayedWriteLine(string txt)
        {
            Thread.Sleep(1000);
            Console.WriteLine(txt);
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

            if(File.Exists(PlayablesFile))
                File.Delete(PlayablesFile);


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
            if ((Math.Abs(GetAbscissIndex(target.Absciss) - GetAbscissIndex(striker.Absciss)) <= range) || (Math.Abs(GetAbscissIndex(striker.Absciss) - GetAbscissIndex(target.Absciss)) <= range) &&
                (Math.Abs(target.Ordinate - striker.Ordinate) <= range) || (Math.Abs(striker.Ordinate - target.Ordinate) <= range))
                return true;
            else return false;
        }

		public static Position IsPositionValid(List<IBaseTroop> listOfTroops, Position actualPos, string direction, int indexMatrice)
		{
			int absciss = ConvertFromStringBase26(actualPos.Absciss);
			int ordinate = actualPos.Ordinate;

			switch (direction)
			{
				case ("left"):
				{
					if (absciss - 1 >= 0)
					{
						for (int i = 0 ; i < listOfTroops.Count ; i++)
						{
							if (listOfTroops[i].Position.Absciss == ConvertToStringBase26(absciss - 1).Replace(" ", "") && listOfTroops[i].Position.Ordinate == ordinate)
								return null;
						}

						Position newPos = new Position();
						newPos.Absciss = ConvertToStringBase26(absciss - 1).Replace(" ", "");
						newPos.Ordinate = ordinate;

						return newPos;
					}
					break;
				}
				case ("right"):
				{
					if (absciss + 1 < indexMatrice)
					{
						for (int i = 0 ; i < listOfTroops.Count ; i++)
						{
							if (listOfTroops[i].Position.Absciss == ConvertToStringBase26(absciss + 1).Replace(" ", "") && listOfTroops[i].Position.Ordinate == ordinate)
								return null;
						}

						Position newPos = new Position();
						newPos.Absciss = ConvertToStringBase26(absciss + 1).Replace(" ", "");
						newPos.Ordinate = ordinate;

						return newPos;
					}
					break;
				}
				case ("up"):
				{
					if (ordinate - 1 >= 1)
					{
						for (int i = 0 ; i < listOfTroops.Count ; i++)
						{
							if (listOfTroops[i].Position.Absciss == actualPos.Absciss && listOfTroops[i].Position.Ordinate == ordinate - 1)
								return null;
						}

						Position newPos = new Position();
						newPos.Absciss = actualPos.Absciss;
						newPos.Ordinate = ordinate - 1;

						return newPos;
					}
					break;
				}
				case ("down"):
				{
					if (ordinate + 1 < indexMatrice + 1)
					{
						for (int i = 0 ; i < listOfTroops.Count ; i++)
						{
							if (listOfTroops[i].Position.Absciss == actualPos.Absciss && listOfTroops[i].Position.Ordinate == ordinate + 1)
								return null;
						}

						Position newPos = new Position();
						newPos.Absciss = actualPos.Absciss;
						newPos.Ordinate = ordinate + 1;

						return newPos;
					}
					break;
				}
				default:
					break;
			}

			return null;
		}

		/// <summary>
		/// Method pour convertir un string en base 26 vers un entier
		/// </summary>
		/// <param name="val">String à convertir</param>
		/// <returns>La valeur entière</returns>
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

		/// <summary>
		/// Method pour convertir un entier vers un string en base 26
		/// </summary>
		/// <param name="index">Entier à convertir</param>
		/// <returns>Le string en base 26</returns>
		public static string ConvertToStringBase26(int index)
		{
			string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			string value;

			if (index < 26)
				value = letters[index].ToString() + ' ';
			else
			{
				int firstIndex = index / 26 - 1;
				int secondIndex = index - 26 * (int)(index / 26);
				value = letters[firstIndex].ToString() + letters[secondIndex].ToString();
			}

			return value;
		}

		/// <summary>
		/// Method pour renvoyer un entier aléatoire compris entre deux valeurs
		/// </summary>
		/// <param name="minVal">Valeur minimum du nombre aléatoire</param>
		/// <param name="maxVal">Valeur maximum du nombre aléatoire</param>
		/// <returns>L'entier aléatoire</returns>
		public static int GenerateRandom(int minVal, int maxVal)
		{
			Random random = new Random();

			int result;

			result = random.Next(minVal, maxVal);

			return result;
		}
		#endregion
	}
}
