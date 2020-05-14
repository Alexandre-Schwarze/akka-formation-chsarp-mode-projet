using StarWars.Managers;
using System;
using System.Runtime.InteropServices;

namespace StarWars
{
	class Program
	{

		#region Attributes
		[DllImport("kernel32.dll", ExactSpelling = true)]
		private static extern IntPtr GetConsoleWindow();
		private static IntPtr ThisConsole = GetConsoleWindow();
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
		private const int HIDE = 0;
		private const int MAXIMIZE = 3;
		private const int MINIMIZE = 6;
		private const int RESTORE = 9;
		#endregion

		static void Main(string[] args)
		{
			SetConsolePosition();
			MainManager.Welcome();
			MainManager.MainMenu(); 
		}

		static void SetConsolePosition()
		{
			Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
			ShowWindow(ThisConsole, MAXIMIZE);
		}
	}
}
