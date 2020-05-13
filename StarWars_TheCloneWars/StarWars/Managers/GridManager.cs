using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using StarWars.Objects;
using System.Xml;
using System.Xml.Linq;

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
		/// Method pour choisir la taille de la grille du jeu
		/// </summary>
		/// <returns>La taille (largeur = hauteur) de la grille</returns>
		public int ChooseIndex()
		{
			int index;

			Console.WriteLine("Choisissez la taille de la grille de jeu : ");
			int.TryParse(Console.ReadLine(), out index);

			return index;
		}

		/// <summary>
		/// Method pour générer la grille du jeu
		/// </summary>
		/// <param name="index">Nombre de lignes et de colonnes de la grille de jeu</param>
		/// <returns></returns>
		public Grid GenereateGrid(int index)
		{
			Grid = new Grid();

			Grid.InitIcons();

			Grid.Index = index;

			Grid.Matrice = new string[index + 2];
			string line = new string('_', index + 2);
			string column = new string(' ', index);

			//
			// Il faut ajouter les personnages dans le string column
			//

			for (int x = 0 ; x < index + 2 ; x++)
			{
				if (x == 0 || x == index + 1)
					Grid.Matrice[x] = line;
				else
					Grid.Matrice[x] = '|' + column + '|';
			}

			return Grid;
		}

		/// <summary>
		/// Method pour charger la grille du jeu depuis un fichier XML
		/// </summary>
		/// <param name="grid">Grille à charger</param>
		public void LoadGrid(Grid grid)
		{
			string fileName = "file.xml";
			string currentDirectory = Directory.GetCurrentDirectory();

			XmlDocument xml = new XmlDocument();
			xml.Load(Path.Combine(currentDirectory, fileName));
			XmlNode node = xml.DocumentElement.SelectSingleNode("Grid");

			string matrice = node.InnerText;
			string[] line = matrice.Split('\n');

			Grid.Index = line[0].Length;
		}
		#endregion
	}
}