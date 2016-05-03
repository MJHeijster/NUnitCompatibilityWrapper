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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NunitCompatibilityWrapper
{
    /// <summary>
    /// Class Program.
    /// </summary>
    class Program
    {
        /// <summary>
        /// The file name
        /// </summary>
        private static string fileName;

        /// <summary>
        /// The where query
        /// </summary>
        private static string whereQuery = "--where=\"";

        /// <summary>
        /// The formatted arguments
        /// </summary>
        private static string newArguments;

        /// <summary>
        /// Boolean if the "where" query should be added.
        /// </summary>
        private static bool useWhere = false;

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            fileName = args[0];

            foreach (var arg in args)
            {
                if (arg.StartsWith("-xml"))
                {
                    newArguments = newArguments + arg.Replace("-xml", "--result").Replace(".xml", ".xml;format=nunit2");
                }
                else if (arg.ToLower().StartsWith("-exclude"))
                {
                    var excludedCategories = arg.Replace("-exclude=", "").Split(',');
                    AddCategories(excludedCategories, false);
                }
                else if (arg.StartsWith("-include"))
                {
                    var includedCategories = arg.Replace("-include=", "").Split(',');
                    AddCategories(includedCategories, true);
                }
                else if (arg != fileName)
                {
                    newArguments = newArguments + "-" + arg;
                }
            }
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = "nunit3-console.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            if (useWhere)
            {
                startInfo.Arguments = fileName + " " + newArguments + " " + whereQuery + "\" -v";
            }
            else
            {
                startInfo.Arguments = fileName + " " + newArguments + " -v";
            }
            using (Process exeProcess = Process.Start(startInfo))
            {
                exeProcess.WaitForExit();
            }
        }

        /// <summary>
        /// Adds the categories.
        /// </summary>
        /// <param name="categories">The categories.</param>
        /// <param name="include">If set to <c>true</c>, the categories should be included.</param>
        private static void AddCategories(string[] categories, bool include)
        {
            useWhere = true;
            if (whereQuery != "--where=\"")
            {
                whereQuery = whereQuery + " and ";
            }
            whereQuery = whereQuery + "(";
            foreach (var category in categories)
            {
                category.Replace("-", "");

                AddToWhereQuery(category, include);

            }
            whereQuery = whereQuery + ")";
        }

        /// <summary>
        /// Adds to the where query.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="include">If set to <c>true</c>, the categories should be included.</param>
        private static void AddToWhereQuery(string category, bool include)
        {
            if (whereQuery != "--where=\"(")
            {
                whereQuery = include ? whereQuery + " or " : whereQuery + " and ";
            }
            if (include)
            {
                whereQuery = whereQuery + "Category == " + category;
            }
            else
            {
                whereQuery = whereQuery + "Category != " + category;
            }
        }
    }
}
