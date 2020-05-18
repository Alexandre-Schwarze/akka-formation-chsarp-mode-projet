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
	public static class GridManager
	{
		#region Ctor

		#endregion

		#region Methods
		/// <summary>
		/// Method pour afficher la grille générée
		/// </summary>
		public static void DisplayGrid()
		{
			GenereateGrid(10);

			for (int x = 0 ; x < (10 * 2) + 1 ; x++)
			{
				string line = "";

				for (int y = 0 ; y < (10 * 2) + 1 ; y++)
				{
					line += Grid.Matrice[x, y];
				}

				Console.WriteLine(line);
			}
		}

		/// <summary>
		/// Method pour choisir la taille de la grille du jeu
		/// </summary>
		/// <returns>La taille (largeur = hauteur) de la grille</returns>
		public static int ChooseIndex()
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
		public static Grid GenereateGrid(int index)
		{
			Grid Grid = new Grid();

			Grid.InitIcons();

			Grid.Index = index;
			Grid.Matrice = new char[(index * 2) + 1, (index * 2) + 1];

			for (int x = 0 ; x < (index * 2) + 1 ; x++)
			{
				for (int y = 0 ; y < (index * 2) + 1 ; y++)
				{
					/*if (x == 0 || y == 0)
					{
						if (y == 0 && x % 2 != 0)
							Grid.Matrice[x, y] = 'A';
						else if (x == 0 && y % 2 != 0)
							Grid.Matrice[x, y] = '1';
					}
					else*/
					//{
						if (x % 2 == 0)
							Grid.Matrice[x, y] = '—';
						else if (x % 2 != 0 && y % 2 == 0)
							Grid.Matrice[x, y] = '|';
						else
							Grid.Matrice[x, y] = ' ';
					//}
				}
			}

			return Grid;
		}

		/// <summary>
		/// Method pour charger la grille du jeu depuis un fichier XML
		/// </summary>
		/// <param name="grid">Grille à charger</param>
		public static void LoadGrid(Grid grid)
		{
			string fileName = "file.xml";
			string currentDirectory = Directory.GetCurrentDirectory();

			XmlDocument xml = new XmlDocument();
			xml.Load(Path.Combine(currentDirectory, fileName));
			XmlNode node = xml.DocumentElement.SelectSingleNode("Grid");

			string matrice = node.InnerText;
			string[] line = matrice.Split('\n');

			grid.Index = line[0].Length;
		}
		#endregion
	}
}