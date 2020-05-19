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
		#region Attributes
		private static bool isTroopsInitialized = false;
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

			int abscissAxis = (index * 2) + 3;
			int ordinateAxis = (index * 4) + 5;

			int abscissValue = 0;
			int ordinateValue = 1;

			Grid.Index = index;
			Grid.Matrice = new char[abscissAxis, ordinateAxis];

			if (!isTroopsInitialized)
			{
				PlaceTroops(listOfTroops, index - 1);
				isTroopsInitialized = true;
			}

			for (int x = 0 ; x < abscissAxis ; x++)
			{
				lines.Add("");

				for (int y = 0 ; y < ordinateAxis ; y++)
				{
					if (x < 2 && y < 4 || x == 1 || y == 2 || y == 3)
						Grid.Matrice[x, y] = ' ';
					else if (x == 0)
					{
						if ((y + 2) % 4 == 0)
							Grid.Matrice[x, y] = Tools.Tools.ConvertToStringBase26(abscissValue)[0];
						else if ((y + 1) % 4 == 0)
						{
							Grid.Matrice[x, y] = Tools.Tools.ConvertToStringBase26(abscissValue)[1];
							abscissValue++;
						}
						else
							Grid.Matrice[x, y] = ' ';
					}
					else if (y < 2)
					{
						if (x % 2 != 0)
						{
							if (y == 0)
								Grid.Matrice[x, y] = ordinateValue.ToString()[0];
							else
							{
								if (ordinateValue > 9)
									Grid.Matrice[x, y] = ordinateValue.ToString()[1];
								else
									Grid.Matrice[x, y] = ' ';

								ordinateValue++;
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
							bool isTroopFind = false;
							string valAbsciss = (Grid.Matrice[0, y].ToString() + Grid.Matrice[0, y + 1].ToString()).Replace(" ", "");
							string valOrdinate = (Grid.Matrice[x, 0].ToString() + Grid.Matrice[x, 1].ToString()).Replace(" ", "");
							int.TryParse(valOrdinate, out int resOrdinate);

							for (int i = 0 ; i < listOfTroops.Count ; i++)
							{
								if (listOfTroops[i].Position.absciss == valAbsciss && listOfTroops[i].Position.ordinate == resOrdinate)
								{
									Grid.Matrice[x, y] = listOfTroops[i].Icon;
									isTroopFind = true;
									break;
								}
							}

							if (!isTroopFind)
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

		/// <summary>
		/// Method pour récupérer la position initiale des personnages
		/// </summary>
		/// <param name="grid">Grille du jeu</param>
		/// <param name="baseTroop">Personnage dont on veut récupérer la position</param>
		/// <param name="firstIndex">Premier index de la matrice</param>
		/// <param name="secondIndex">Second index de la matrice</param>
		public static void SetTroopPosition(Grid grid, IBaseTroop baseTroop, int firstIndex, int secondIndex)
		{
			if (baseTroop.Position == null)
				baseTroop.Position = new Tools.Position();

			string absciss;
			int ordinate;

			if (grid.Matrice[0, secondIndex + 1] != ' ')
				absciss = grid.Matrice[0, secondIndex].ToString() + grid.Matrice[0, secondIndex + 1].ToString();
			else
				absciss = grid.Matrice[0, secondIndex].ToString();
			
			if (grid.Matrice[firstIndex, 1] != ' ')
				int.TryParse(grid.Matrice[firstIndex, 0].ToString() + grid.Matrice[firstIndex, 1].ToString(), out ordinate);
			else
				int.TryParse(grid.Matrice[firstIndex, 0].ToString(), out ordinate);

			baseTroop.Position.absciss = absciss;
			baseTroop.Position.ordinate = ordinate;
		}

		/// <summary>
		/// Method pour détecter un ennemi à proximité
		/// </summary>
		/// <param name="listOfTroops">Liste des troupes</param>
		/// <param name="baseTroop">Troupe qui cherche un ennemi</param>
		/// <param name="grid">Grille du jeu</param>
		/// <returns>Une troupe ennemie détectée ou null si rien n'est détecté</returns>
		public static IBaseTroop CheckAroundForTroop(List<IBaseTroop> listOfTroops, IBaseTroop baseTroop)
		{
			int absciss = Tools.Tools.ConvertFromStringBase26(baseTroop.Position.absciss);
			int ordinate = baseTroop.Position.ordinate;

			for (int x = ordinate - 1 ; x <= ordinate + 1 ; x++)
			{
				if (x > 0)
				{
					for (int y = absciss - 1 ; y <= absciss + 1 ; y++)
					{
						if (y >= 0)
						{
							for (int i = 0 ; i < listOfTroops.Count ; i++)
							{
								if (listOfTroops[i].Position.absciss == Tools.Tools.ConvertToStringBase26(y).Replace(" ", "") && listOfTroops[i].Position.ordinate == x && listOfTroops[i].Forceside != baseTroop.Forceside)
									return listOfTroops[i];
							}
						}
					}
				}
			}

			return null;
		}

		private static void PlaceTroops(List<IBaseTroop> listOfTroops, int indexMatrice)
		{
			List<int> listOfAbsciss = Tools.Tools.GenerateRandoms(listOfTroops.Count, 0, indexMatrice);
			List<int> listOfOrdinate = Tools.Tools.GenerateRandoms(listOfTroops.Count, 0, indexMatrice / 2);

			for (int i = 0 ; i < listOfTroops.Count ; i++)
			{
				if (!isTroopsInitialized)
					listOfTroops[i].Position = new Tools.Position();

				listOfTroops[i].Position.absciss = Tools.Tools.ConvertToStringBase26(listOfAbsciss[i]).Replace(" ", "");
				listOfTroops[i].Position.ordinate = listOfOrdinate[i];
			}

			/*Random testTroop = new Random();

			if (testTroop.Next(0, 20) > 18 && indexTroops < listOfTroops.Count)
			{
				grid.Matrice[firstIndex, secondIndex] = listOfTroops[indexTroops].Icon;

				SetTroopPosition(grid, listOfTroops[indexTroops], firstIndex, secondIndex);

				indexTroops++;
			}
			else
				grid.Matrice[firstIndex, secondIndex] = ' ';*/
		}
		#endregion
	}
}