using StarWars.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarWars
{
	class Program
	{
		public static char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
		#region Attributes

		#endregion

		static void Main(string[] args)
		{
			Init();
			MainManager.Welcome();
			MainManager.MainMenu(); 
		}

		static void Init()
		{
			Tools.Tools.SetConsolePosition();
			Tools.Tools.InitFolders();
			EntitiesManager.InitCharactersTypes();
		}
	}
}
