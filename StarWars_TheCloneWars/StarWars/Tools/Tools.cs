﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace StarWars.Tools
{
    /// <summary>
    /// Type personnalisé pour représenter le côté de la Force des participants
    /// </summary>
    public enum ForceSide
    {
        Light = 0,
        Dark = 1
    }

    /// <summary>
    /// Type personnalisé pour les positions au sein de la grille
    /// </summary>
    public class Position
    {
        char absciss;
        int ordinate;
    }

    /// <summary>
    /// Classe utilitaire 
    /// </summary>
    public static class Tools
    {
        #region Outillages Console
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

        public static void RightOffsetWriteLine(string txt)
        {
            string offset = String.Empty;

            for (int i = 0; i < txtoffset; i++)
                offset += " ";

            Console.WriteLine(offset + txt);
        }

        public static void SetConsolePosition()
        {
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            ShowWindow(ThisConsole, MAXIMIZE);

        }
        #endregion


        #region Outillage Dossiers
        private static string UserAppFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static string GameFolder = UserAppFolderPath + "\\CloneWars";
        public static string SaveFolder = GameFolder + "\\Saves";
        public static string DataFolder = GameFolder + "\\Data";
        public static string PlayablesFile = "PJs.xml";

        public static void InitFolders()
        {
            System.IO.Directory.CreateDirectory(GameFolder);
            System.IO.Directory.CreateDirectory(SaveFolder);
            System.IO.Directory.CreateDirectory(DataFolder);
        }
        #endregion
    }
}
