/*++

Copyright (C) Institute of Biochemistry and Biophysics, Polish Academy of Sciences.

Module Name:

    Program.cs

Abstract:

    This module contains the main entry point for the NMEA Translator App.

Environment:

    Microsoft .NET Framework

--*/
using System;
using System.Windows.Forms;

namespace NMEATranslator
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
