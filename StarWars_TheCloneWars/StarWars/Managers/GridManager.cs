using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using StarWars.Objects;
using System.Xml;
using System.Xml.Linq;
using StarWars.Entities.Interfaces;
using System.Linq;
using MoreLinq;

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
			Console.Clear();

			List<string> lines = new List<string>();
			IBaseTroop player = null;

			foreach (IBaseTroop troop in listOfTroops)
			{
				if (troop.IsPlayable == 0)
					player = troop;
			}

			Tools.Tools.RightOffsetWriteLine("			PERSONNAGE : " + player.GetType().Name + "		||		PV : " + player.Remaining_HP + "/" + player.MaxHP);

			Grid grid = GenerateGrid(listOfTroops, index, lines);

			for (int x = 0 ; x < lines.Count ; x++)
			{
				for (int y = 0 ; y < lines[x].Length ; y++)
				{
					if (x == 0 || y == 0 || y == 1)
						Console.ForegroundColor = ConsoleColor.Blue;
					else if (lines[x][y] == 'A' || lines[x][y] == 'S' || lines[x][y] == 'D' || lines[x][y] == 'J' || lines[x][y] == 'W' || lines[x][y] == 'Y')
					{
						IBaseTroop PJ = listOfTroops.Where(s => s.Icon == lines[x][y]).First();
						Console.ForegroundColor = PJ.color;
					}
					else if (lines[x][y] == '8' || lines[x][y] == 'O')
					{
						IBaseTroop trooper = listOfTroops.Where(s => s.Icon == lines[x][y]).First();
						Console.ForegroundColor = trooper.color;
					}
					else if (lines[x][y] == '#' || lines[x][y] == '§')
					{
						IBaseTroop droid = listOfTroops.Where(s => s.Icon == lines[x][y]).First();
						Console.ForegroundColor = droid.color;
					}
					else
						Console.ForegroundColor = ConsoleColor.White;

					Console.Write(lines[x][y]);
				}
				Console.Write("\r\n");
			}

			string leg = "légende : ";
			listOfTroops.DistinctBy((i) => i.Icon).ToList().ForEach((e) => leg += e.Icon + " = "+ e.GetType().Name + ", ");
			Console.WriteLine(leg);

			Console.WriteLine("\r\n\r\n");

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

			while (index > 70 || index < 15)
			{
				Console.WriteLine("Choisissez une taille supérieure à 15 et inférieure à 70 : ");
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
								if (listOfTroops[i].Position.Absciss == valAbsciss && listOfTroops[i].Position.Ordinate == resOrdinate)
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
		/// Method pour placer les troupes à chaque refresh de la grille
		/// </summary>
		/// <param name="listOfTroops"></param>
		/// <param name="indexMatrice"></param>
		private static void PlaceTroops(List<IBaseTroop> listOfTroops, int indexMatrice)
		{
			for (int i = 0 ; i < listOfTroops.Count ; i++)
			{
				if (!isTroopsInitialized)
					listOfTroops[i].Position = new Tools.Position();

				int absciss = Tools.Tools.GenerateRandom(0, indexMatrice);
				int ordinate;

				if (listOfTroops[i].Forceside == Tools.ForceSide.Dark)
					ordinate = Tools.Tools.GenerateRandom(1, (indexMatrice / 2) + 1);
				else
					ordinate = Tools.Tools.GenerateRandom((indexMatrice / 2) + 2, indexMatrice - 1);

				listOfTroops[i].Position.Absciss = Tools.Tools.ConvertToStringBase26(absciss).Replace(" ", "");
				listOfTroops[i].Position.Ordinate = ordinate;
			}
		}

		/// <summary>
		/// Method pour détecter un ennemi à proximité
		/// </summary>
		/// <param name="listOfTroops">Liste des troupes</param>
		/// <param name="baseTroop">Troupe qui cherche un ennemi</param>
		/// <param name="grid">Grille du jeu</param>
		/// <returns>Une troupe ennemie détectée ou null si rien n'est détecté</returns>
		public static IBaseTroop CheckAroundForTroop(List<IBaseTroop> listOfTroops, IBaseTroop baseTroop, int indexMatrice)
		{
			int absciss = Tools.Tools.ConvertFromStringBase26(baseTroop.Position.Absciss);
			int ordinate = baseTroop.Position.Ordinate;

			if (absciss - 1 >= 0)
			{
				for (int i = 0 ; i < listOfTroops.Count ; i++)
				{
					if (listOfTroops[i].Position.Absciss == Tools.Tools.ConvertToStringBase26(absciss - 1).Replace(" ", "") && listOfTroops[i].Position.Ordinate == ordinate && listOfTroops[i].Forceside != baseTroop.Forceside)
						return listOfTroops[i];
				}
			}
			if (absciss + 1 < indexMatrice)
			{
				for (int i = 0 ; i < listOfTroops.Count ; i++)
				{
					if (listOfTroops[i].Position.Absciss == Tools.Tools.ConvertToStringBase26(absciss + 1).Replace(" ", "") && listOfTroops[i].Position.Ordinate == ordinate && listOfTroops[i].Forceside != baseTroop.Forceside)
						return listOfTroops[i];
				}
			}
			if (ordinate - 1 >= 1)
			{
				for (int i = 0 ; i < listOfTroops.Count ; i++)
				{
					if (listOfTroops[i].Position.Absciss == baseTroop.Position.Absciss && listOfTroops[i].Position.Ordinate == ordinate - 1 && listOfTroops[i].Forceside != baseTroop.Forceside)
						return listOfTroops[i];
				}
			}
			if (ordinate + 1 < indexMatrice + 1)
			{
				for (int i = 0 ; i < listOfTroops.Count ; i++)
				{
					if (listOfTroops[i].Position.Absciss == baseTroop.Position.Absciss && listOfTroops[i].Position.Ordinate == ordinate + 1 && listOfTroops[i].Forceside != baseTroop.Forceside)
						return listOfTroops[i];
				}
			}

			return null;
		}

		/// <summary>
		/// Method pour détecter un personnage à proximité avant de tenter de se déplacer
		/// </summary>
		/// <param name="listOfTroops">Liste des troupes</param>
		/// <param name="baseTroop">Troupe qui cherche un ennemi</param>
		/// <returns>Une troupe ennemie détectée ou null si rien n'est détecté</returns>
		public static Tools.Position CheckAroundForTroopMove(List<IBaseTroop> listOfTroops, IBaseTroop baseTroop, int indexMatrice)
		{
			List<Tools.Position> listOfValidPos = new List<Tools.Position>();

			Tools.Position newPosLeft = Tools.Tools.IsPositionValid(listOfTroops, baseTroop.Position, "left", indexMatrice);
			Tools.Position newPosRight = Tools.Tools.IsPositionValid(listOfTroops, baseTroop.Position, "right", indexMatrice);
			Tools.Position newPosUp = Tools.Tools.IsPositionValid(listOfTroops, baseTroop.Position, "up", indexMatrice);
			Tools.Position newPosDown = Tools.Tools.IsPositionValid(listOfTroops, baseTroop.Position, "down", indexMatrice);

			if (newPosLeft != null)
				listOfValidPos.Add(newPosLeft);
			if (newPosRight != null)
				listOfValidPos.Add(newPosRight);
			if (newPosUp != null)
				listOfValidPos.Add(newPosUp);
			if (newPosDown != null)
				listOfValidPos.Add(newPosDown);

			if (listOfValidPos.Count == 0)
				return null;

			Random random = new Random();
			Tools.Position newPosition = listOfValidPos[random.Next(0, listOfValidPos.Count)];

			return newPosition;
		}

		public static Tools.Position CheckPlayer(List<IBaseTroop> listOfTroops, Tools.Position position, ConsoleKeyInfo key, int indexMatrice)
		{
			switch (key.Key)
			{
				case ConsoleKey.LeftArrow:
					return Tools.Tools.IsPositionValid(listOfTroops, position, "left", indexMatrice);
				case ConsoleKey.UpArrow:
					return Tools.Tools.IsPositionValid(listOfTroops, position, "up", indexMatrice);
				case ConsoleKey.RightArrow:
					return Tools.Tools.IsPositionValid(listOfTroops, position, "right", indexMatrice);
				case ConsoleKey.DownArrow:
					return Tools.Tools.IsPositionValid(listOfTroops, position, "down", indexMatrice);
				default:
					break;
			}

			return null;
		}
		#endregion
	}
}