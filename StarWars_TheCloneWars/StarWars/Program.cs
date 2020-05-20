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
		}

		static void Init()
		{
			Console.OutputEncoding = System.Text.Encoding.UTF8;
			Tools.Tools.SetConsolePosition();
			Tools.Tools.InitFolders();
			EntitiesManager.InitCharactersTypes();
		}
	}
}
