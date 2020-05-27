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
using StarWars.Tools;

namespace StarWars.Managers
{
	/// <summary>
	/// Manager pour la création de la carte du jeu
	/// </summary>
	public sealed class GridManager
	{
		#region Attributs
		private static GridManager _instance;
		static readonly object instanceLock = new object();
		public static GridManager Instance
		{
			get
			{
				if (_instance == null) //Les locks prennent du temps, il est préférable de vérifier d'abord la nullité de l'instance.
				{
					lock (instanceLock)
					{
						if (_instance == null) //on vérifie encore, au cas où l'instance aurait été créée entretemps.
							_instance = new GridManager();
					}
				}
				return _instance;
			}
		}
		private static readonly int[] gridIndex = { 15, 27 };
		public static bool isTroopsInitialized = false;
		#endregion

		#region Ctor
		private GridManager() { }
		#endregion


		#region Methods
		/// <summary>
		/// Method pour afficher la grille générée
		/// </summary>
		public Grid DisplayGrid(List<IBaseTroop> listOfTroops, IBaseTroop player, int index)
		{
			Console.Clear();

			List<string> lines = new List<string>();

			if (player != null)
			{
				CustomConsole.RightOffsetWriteLine("			PERSONNAGE : " + player.GetType().Name + "		||		PV : " + player.Remaining_HP + "/" + player.MaxHP + "\r\n");
				listOfTroops.Add(player);
			}

			Grid grid = GenerateGrid(listOfTroops, index, lines);

			string testPlayer = "ASDJWY";
			string testTrooper = "8O";
			string testDroid = "#§";

			for (int x = 0 ; x < lines.Count ; x++)
			{
				for (int y = 0 ; y < lines[x].Length ; y++)
				{
					if (x == 0 || y == 0 || y == 1)
						Console.ForegroundColor = ConsoleColor.Blue;
					else if (testPlayer.Contains(lines[x][y]))
					{
						IBaseTroop PJ = listOfTroops.Where(s => s.Icon == lines[x][y]).First();

						if (PJ.Remaining_HP == PJ.MaxHP)
							PJ.color = ConsoleColor.Green;
						else
							PJ.color = ConsoleColor.Red;

						Console.ForegroundColor = PJ.color;
					}
					else if (testTrooper.Contains(lines[x][y]))
					{
						IBaseTroop trooper = listOfTroops.Where(s => s.Icon == lines[x][y]).First();
						Console.ForegroundColor = trooper.color;
					}
					else if (testDroid.Contains(lines[x][y]))
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

			string legend = "légende : ";
			listOfTroops.DistinctBy((i) => i.Icon).ToList().ForEach((e) => legend += e.Icon + " = "+ e.GetType().Name + " | ");
			legend = legend.Remove(legend.Length - 2);
			Console.WriteLine(legend);

			Console.WriteLine("\r\n");
			listOfTroops.Remove(player);
			return grid;
		}

		/// <summary>
		/// Method pour choisir la taille de la grille du jeu
		/// </summary>
		/// <returns>La taille (largeur = hauteur) de la grille</returns>
		public int ChooseIndex()
		{
			int index;

			Console.WriteLine("Choisissez la taille de la grille de jeu : ");
			int.TryParse(Console.ReadLine(), out index);

			while (index < gridIndex[0] || index > gridIndex[1])
			{
				Console.WriteLine("Choisissez une taille supérieure à 15 et inférieure à 27 : ");
				int.TryParse(Console.ReadLine(), out index);
			}

			return index;
		}

		/// <summary>
		/// Method pour générer la grille du jeu et le placement des personnages
		/// </summary>
		/// <param name="index">Nombre de lignes et de colonnes de la grille de jeu</param>
		/// <returns></returns>
		public Grid GenerateGrid(List<IBaseTroop> listOfTroops, int index, List<string> lines)
		{
			Grid Grid = new Grid();

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
		private void PlaceTroops(List<IBaseTroop> listOfTroops, int indexMatrice)
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
		public IBaseTroop CheckAroundForTroop(List<IBaseTroop> listOfTroops, IBaseTroop baseTroop, int indexMatrice)
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
		public Tools.Position CheckAroundForTroopMove(List<IBaseTroop> listOfTroops, IBaseTroop baseTroop, int indexMatrice)
		{
			List<Tools.Position> listOfValidPos = new List<Tools.Position>();

			Tools.Position newPosLeft = Tools.Tools.IsPositionValid(listOfTroops, baseTroop.Position, ConsoleKey.LeftArrow, indexMatrice);
			Tools.Position newPosRight = Tools.Tools.IsPositionValid(listOfTroops, baseTroop.Position, ConsoleKey.RightArrow, indexMatrice);
			Tools.Position newPosUp = Tools.Tools.IsPositionValid(listOfTroops, baseTroop.Position, ConsoleKey.UpArrow, indexMatrice);
			Tools.Position newPosDown = Tools.Tools.IsPositionValid(listOfTroops, baseTroop.Position, ConsoleKey.DownArrow, indexMatrice);

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

		/// <summary>
		/// Method ??
		/// </summary>
		/// <param name="listOfTroops"></param>
		/// <param name="position"></param>
		/// <param name="key"></param>
		/// <param name="indexMatrice"></param>
		/// <returns></returns>
		public Tools.Position CheckPlayer(List<IBaseTroop> listOfTroops, Tools.Position position, ConsoleKeyInfo key, int indexMatrice)
		{
			return Tools.Tools.IsPositionValid(listOfTroops, position, key.Key, indexMatrice);
		}
		#endregion
	}
}