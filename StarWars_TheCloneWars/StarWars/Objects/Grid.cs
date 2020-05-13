using StarWars.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarWars.Objects
{
	/// <summary>
	/// Class de la grille du jeu
	/// </summary>
	public class Grid
	{
		#region Attributes
		public int Index
		{
			get; set;
		}
		public string[] Matrice;

		public Dictionary<string, char> Icons;
		#endregion

		#region Ctor

		#endregion

		#region Methods
		/// <summary>
		/// Method pour initialiser les icones (symboles) des personnages
		/// </summary>
		public void InitIcons()
		{
			Icons = new Dictionary<string, char>()
			{
				{"ObiWan", '8' },
				{"Anakin", '8' },
				{"Yoda", 'o' },
				{"Dooku", 'A' },
				{"Sidious", 'A' },
				{"JangoFett", 'M' }
			};
		}
		#endregion
	}
}
