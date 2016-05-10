using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NunitCompatibilityWrapper.Parameters;

namespace NunitCompatibilityWrapper.Arguments
{
    public static class ArgumentHandler
    {
        public static void HandleArgument(string arg, string fileName, ref string newArguments)
        {
            if (arg.StartsWith("-xml"))
            {
                newArguments = newArguments + arg.Replace("-xml", "--result").Replace(".xml", ".xml;format=nunit2");
            }
            else if (arg.ToLower().StartsWith("-exclude"))
            {
                var excludedCategories = arg.Replace("-exclude=", "").Split(',');
                WhereQuery.AddCategories(excludedCategories, false);
            }
            else if (arg.StartsWith("-include"))
            {
                var includedCategories = arg.Replace("-include=", "").Split(',');
                WhereQuery.AddCategories(includedCategories, true);
            }
            else if (arg != fileName)
            {
                newArguments = newArguments + "-" + arg;
            }
        }
    }
}
