using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using StarWars.Objects;
using System.Xml;
using System.Xml.Linq;
using StarWars.Entities.Interfaces;

namespace StarWars.Managers
{
	/// <summary>
	/// Manager pour la création de la carte du jeu
	/// </summary>
	public static class GridManager
	{
		#region Ctor
		private const string Alphabet = "abcdefghijklmnopqrstuvwxyz";
		#endregion

		#region Methods
		/// <summary>
		/// Method pour afficher la grille générée
		/// </summary>
		public static Grid DisplayGrid(List<IBaseTroop> listOfTroops, int index)
		{
			List<string> lines = new List<string>();

			Grid grid = GenerateGrid(listOfTroops, index, lines);

			for (int x = 0 ; x < lines.Count ; x++)
			{
				Console.WriteLine(lines[x]);
			}

			return grid;
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

			while (index >= 70)
			{
				Console.WriteLine("Choisissez une taille inférieure à 70 : ");
				int.TryParse(Console.ReadLine(), out index);
			}

			return index;
		}

		/// <summary>
		/// Method pour générer la grille du jeu et le placement des personnages
		/// </summary>
		/// <param name="index">Nombre de lignes et de colonnes de la grille de jeu</param>
		/// <returns></returns>
		public static Grid GenerateGrid(List<IBaseTroop> listOfTroops, int index, List<string> lines)
		{
			Grid Grid = new Grid();

			Grid.InitIcons();

			int ordinateAxis = (index * 2) + 3;
			int abscissaAxis = (index * 4) + 5;

			int ordinateValue = 0;
			int abscissaValue = 1;

			Grid.Index = index;
			Grid.Matrice = new char[ordinateAxis, abscissaAxis];

			int indexTroops = 0;

			for (int x = 0 ; x < ordinateAxis ; x++)
			{
				lines.Add("");

				for (int y = 0 ; y < abscissaAxis ; y++)
				{
					if (x < 2 && y < 4 || x == 1 || y == 2 || y == 3)
						Grid.Matrice[x, y] = ' ';
					else if (x == 0)
					{
						if ((y + 2) % 4 == 0)
							Grid.Matrice[x, y] = Tools.Tools.ConvertToStringBase26(ordinateValue)[0];
						else if ((y + 1) % 4 == 0)
						{
							Grid.Matrice[x, y] = Tools.Tools.ConvertToStringBase26(ordinateValue)[1];
							ordinateValue++;
						}
						else
							Grid.Matrice[x, y] = ' ';
					}
					else if (y < 2)
					{
						if (x % 2 != 0)
						{
							if (y == 0)
								Grid.Matrice[x, y] = abscissaValue.ToString()[0];
							else
							{
								if (abscissaValue > 9)
									Grid.Matrice[x, y] = abscissaValue.ToString()[1];
								else
									Grid.Matrice[x, y] = ' ';

								abscissaValue++;
							}
						}
						else
							Grid.Matrice[x, y] = ' ';
					}
					else
					{
						if (x % 2 == 0)
							Grid.Matrice[x, y] = '—';
						else if (x % 2 != 0 && y % 4 == 0)
							Grid.Matrice[x, y] = '|';
						else if ((y + 2) % 4 == 0) {
							Random testTroop = new Random();

							if (testTroop.Next(0, 20) > 18 && indexTroops < listOfTroops.Count)
							{
								Grid.Matrice[x, y] = listOfTroops[indexTroops].Icon;

								/*if (Grid.Matrice[0, y + 1] != ' ')
									listOfTroops[indexTroops].Position.absciss = Grid.Matrice[0, y].ToString() + Grid.Matrice[0, y + 1].ToString();
								else
									listOfTroops[indexTroops].Position.absciss = Grid.Matrice[0, y].ToString();

								if (Grid.Matrice[x, 1] != ' ')
									int.TryParse(Grid.Matrice[x, 0].ToString() + Grid.Matrice[x, 1].ToString(), out listOfTroops[indexTroops].Position.ordinate);
								else
									int.TryParse(Grid.Matrice[x, 0].ToString(), out listOfTroops[indexTroops].Position.ordinate);*/

								indexTroops++;
							}
							else
								Grid.Matrice[x, y] = ' ';
						}
						else
						{
							Grid.Matrice[x, y] = ' ';
						}
					}

					lines[x] += Grid.Matrice[x, y];
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