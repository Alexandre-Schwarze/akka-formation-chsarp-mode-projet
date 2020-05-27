using Microsoft.Extensions.Configuration;
using StarWars.Managers;
using StarWars.Tools;
using System;
using System.Collections.Generic;
using System.IO;
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

			var builder = new ConfigurationBuilder()
			   .SetBasePath(Directory.GetCurrentDirectory())
			   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
			   .AddUserSecrets<Program>()
			   .AddEnvironmentVariables();

			IConfigurationRoot configuration = builder.Build();
			var mySettingsConfig = new MySettingsConfig();
			configuration.GetSection("MySettings").Bind(mySettingsConfig);

			Console.WriteLine("Setting from appsettings.json: " + mySettingsConfig.AccountName);
			Console.WriteLine("Setting from secrets.json: " + mySettingsConfig.ApiSecret);
			Console.WriteLine("Connection string: " + configuration.GetConnectionString("DefaultConnection"));
			BDDManager.Instance.connexionstring = configuration.GetConnectionString("DefaultConnection");
			Console.WriteLine("Enter key to continue ...");
			Console.ReadKey();
		}
	}
}
