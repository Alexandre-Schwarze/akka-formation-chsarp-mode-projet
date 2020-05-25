using StarWars.Managers;
using StarWars.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarWars
{
	class Program
	{
		static void Main(string[] args)
		{
            Init();
            MainManager.Instance.Welcome();
        }

		static void Init()
		{
			Console.OutputEncoding = System.Text.Encoding.UTF8;
			CustomConsole.SetConsolePosition();
			Tools.Tools.InitFolders();
			EntitiesManager.Instance.InitCharactersTypes();
		}
	}
}
