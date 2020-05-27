using StarWars.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

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
    /// Classe utilitaire 
    /// </summary>
    public static class Tools
    {
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
		private static string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
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

		/// <summary>
		/// Renvoie si une position est valide autour du personnage
		/// </summary>
		/// <param name="listOfTroops">Liste des troupes</param>
		/// <param name="actualPos">Position actuel du personnage</param>
		/// <param name="key">Touche entrée par le joueur ou l'ordinateur</param>
		/// <param name="indexMatrice">Taille de la grille</param>
		/// <returns>Une position valide (null si aucune position trouvée)</returns>
		public static Position IsPositionValid(List<IBaseTroop> listOfTroops, Position actualPos, ConsoleKey key, int indexMatrice)
		{
			int absciss = ConvertFromStringBase26(actualPos.Absciss);
			int ordinate = actualPos.Ordinate;

			switch (key)
			{
				case (ConsoleKey.LeftArrow):
				{
					if (absciss - 1 >= 0)
						return SetNewPosition(listOfTroops, absciss - 1, ordinate);
					break;
				}
				case (ConsoleKey.RightArrow):
				{
					if (absciss + 1 < indexMatrice)
						return SetNewPosition(listOfTroops, absciss + 1, ordinate);
					break;
				}
				case (ConsoleKey.UpArrow):
				{
					if (ordinate - 1 >= 1)
						return SetNewPosition(listOfTroops, absciss, ordinate - 1);
					break;
				}
				case (ConsoleKey.DownArrow):
				{
					if (ordinate + 1 < indexMatrice + 1)
						return SetNewPosition(listOfTroops, absciss, ordinate + 1);
					break;
				}
				default:
					break;
			}

			return null;
		}

		/// <summary>
		/// Renvoie une nouvelle position
		/// </summary>
		/// <param name="listOfTroops">Liste des troupes</param>
		/// <param name="absciss">Abscisse de la nouvelle position</param>
		/// <param name="ordinate">Ordonnée de la nouvelle position</param>
		/// <returns>La nouvelle position (null si la position est occupée)</returns>
		private static Position SetNewPosition(List<IBaseTroop> listOfTroops, int absciss, int ordinate)
		{
			for (int i = 0 ; i < listOfTroops.Count ; i++)
			{
				if (listOfTroops[i].Position.Absciss == ConvertToStringBase26(absciss).Replace(" ", "") && listOfTroops[i].Position.Ordinate == ordinate)
					return null;
			}

			Position newPos = new Position();
			newPos.Absciss = ConvertToStringBase26(absciss).Replace(" ", "");
			newPos.Ordinate = ordinate;

			return newPos;
		}

		/// <summary>
		/// Method pour convertir un string en base 26 vers un entier
		/// </summary>
		/// <param name="val">String à convertir</param>
		/// <returns>La valeur entière</returns>
		public static int ConvertFromStringBase26(string val)
		{
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
