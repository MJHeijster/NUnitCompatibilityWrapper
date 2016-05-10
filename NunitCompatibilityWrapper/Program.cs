// ***********************************************************************
// Assembly         : NunitCompatibilityWrapper
// Author           : Jeroen Heijster
// Created          : 05-03-2016
//
// Last Modified By : Jeroen Heijster
// Last Modified On : 05-03-2016
// ***********************************************************************
// <copyright file="Program.cs">
//     Copyright ©  2016
// </copyright>
// <summary>A wrapper for NUnit</summary>
// ***********************************************************************

using System;
using System.Diagnostics;
using System.Linq;

using NunitCompatibilityWrapper.Arguments;
using NunitCompatibilityWrapper.Parameters;

namespace NunitCompatibilityWrapper
{
    /// <summary>
    /// Class Program.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The file name
        /// </summary>
        private static string fileName;

        /// <summary>
        /// The formatted arguments
        /// </summary>
        private static string newArguments;

        /// <summary>
        /// The NUnit console file name
        /// </summary>
        private const string nUnitConsoleFileName = "nunit-console.exe";

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static void Main(string[] args)
        {
            fileName = args[0];

            var argList = args.ToList();
            argList.Sort();
            foreach (var arg in argList)
            {
                ArgumentHandler.HandleArgument(arg, fileName, ref newArguments);
            }

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = nUnitConsoleFileName;
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            if (WhereQuery.useWhere)
            {
                startInfo.Arguments = fileName + " " + newArguments + " " + WhereQuery.whereQuery + "\" -v";
            }
            else
            {
                startInfo.Arguments = fileName + " " + newArguments + " -v";
            }
            Console.WriteLine("Calling " + startInfo.FileName + " with arguments: " + startInfo.Arguments);
            using (Process exeProcess = Process.Start(startInfo))
            {
                exeProcess.WaitForExit();
            }
        }
    }
}