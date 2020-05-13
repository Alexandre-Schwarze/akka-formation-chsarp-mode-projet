using System;
using System.Collections.Generic;
using System.Text;
using StarWars.Objects;

namespace StarWars.Managers
{
	/// <summary>
	/// Manager pour la création de la carte du jeu
	/// </summary>
	public class GridManager
	{
		#region Attributes
		public Grid Grid
		{
			get; set;
		}
		#endregion

		#region Ctor
		
		#endregion

		#region Methods
		/// <summary>
		/// Method pour générer la grille du jeu
		/// </summary>
		/// <param name="index">Nombre de lignes et de colonnes de la grille de jeu</param>
		/// <returns></returns>
		public Grid GenereateGrid(int index)
		{
			Grid.Index = index;

			string line = new string('_', index + 2);
			string column = new string(' ', index);

			Grid.lineWall = line;
			Grid.columnWall = column;

			return Grid;
		}

		/// <summary>
		/// Method pour charger la grille du jeu
		/// </summary>
		/// <param name="grid">Grille à charger</param>
		public void Load_Grid(Grid grid)
		{

		}
		#endregion
	}
}