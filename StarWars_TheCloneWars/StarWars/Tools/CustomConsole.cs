using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace StarWars.Tools
{
    public sealed class CustomConsole
    {
        #region Attributs
        private static CustomConsole _instance;
        static readonly object instanceLock = new object();
        public static CustomConsole Instance
        {
            get
            {
                if (_instance == null) //Les locks prennent du temps, il est préférable de vérifier d'abord la nullité de l'instance.
                {
                    lock (instanceLock)
                    {
                        if (_instance == null) //on vérifie encore, au cas où l'instance aurait été créée entretemps.
                            _instance = new CustomConsole();
                    }
                }
                return _instance;
            }
        }
        [DllImport("kernel32.dll", ExactSpelling = true)]
        public static extern IntPtr GetConsoleWindow();
        public static IntPtr ThisConsole = GetConsoleWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        public const int HIDE = 0;
        public const int MAXIMIZE = 3;
        public const int MINIMIZE = 6;
        public const int RESTORE = 9;
        private static int txtoffset = 12;
        #endregion

        #region Ctor
        private CustomConsole() { }
        #endregion

        #region Methods
        public void RightOffsetWriteLine(string txt)
        {
            Console.WriteLine("\r\n");
            string offset = String.Empty;

            for (int i = 0; i < txtoffset; i++)
                offset += " ";

            Console.WriteLine(offset + txt);
        }
        public static void DelayedWriteLine(string txt)
        {
            Thread.Sleep(1000);
            Console.WriteLine(txt);
        }
        public void SetConsolePosition()
        {
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            ShowWindow(ThisConsole, MAXIMIZE);
        }
        public void ClearLastConsoleLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
        //public static void ConsoleWriteLineCheckRedundancy(string txt)
        //{
        //	Console.SetCursorPosition(0, Console.CursorTop - 1);
        //	if (Console.ReadLine() == txt)
        //		return;
        //	else
        //		Console.WriteLine(txt);
        //}
        #endregion


    }
}
