using StarWars.Managers;
using System;

namespace StarWars
{
	class Program
	{

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
