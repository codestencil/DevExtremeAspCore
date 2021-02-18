using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeraSystems.CodeStencil.Contracts;
using ZeraSystems.CodeNanite.Expansion;


namespace ZeraSystems.DevExtremeAspCore
{
    public static class General
    {
        //public static int StartCol = 4;

        public static bool DisplayColumn(ISchemaItem column, bool allowPrimaryKey=true)
        {
            if (column.IsChecked || (allowPrimaryKey && column.IsPrimaryKey))
                return true;
            else
                return false;
        }

        public static string SplitCamelCase(string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
        }

    }
}
