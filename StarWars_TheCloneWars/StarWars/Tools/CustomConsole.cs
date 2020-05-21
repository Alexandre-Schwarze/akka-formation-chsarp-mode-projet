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
    public static class CustomConsole
    {
        #region Attributs
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

        #region Methods
        public static void RightOffsetWriteLine(string txt)
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
            CustomConsole.RightOffsetWriteLine(txt);
        }
        public static void SetConsolePosition()
        {
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            ShowWindow(ThisConsole, MAXIMIZE);
        }
        public static void ClearLastConsoleLine()
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
        //		CustomConsole.RightOffsetWriteLine(txt);
        //}
        #endregion


    }
}
