using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Humanizer;
using ZeraSystems.CodeNanite.Expansion;
using ZeraSystems.CodeStencil.Contracts;

namespace ZeraSystems.DevExtremeAspCore
{
    public partial class PopulateModel : ExpansionBase
    {
        private string _table;
        private void MainFunction()
        {
            _table = GetTable(Input);
            Output = Populate();
        }

        private string Populate()
        {
            BuildSnippet(null);
            BuildSnippet("private void PopulateModel("+_table+" model, IDictionary values) {",8);

            var columns = GetColumns(_table);
            foreach (var column in columns)
            {
                if (!General.DisplayColumn(column)) continue;

                if (column.ColumnType == "System.Byte[]") continue;
                var col = column.ColumnName.Humanize().Underscore().ToUpper();
                //var fieldName = Pluralize(_table, PreserveTableName()) + "." + column.ColumnName;
                var fieldName = _table + "." + column.ColumnName;
                BuildSnippet("string "+col+" = nameof("+fieldName+");", 12);
            }
            BuildSnippet("");

            foreach (var column in columns)
            {
                if (!General.DisplayColumn(column)) continue;

                if (column.ColumnType == "System.Byte[]") continue;
                var col = column.ColumnName.Humanize().Underscore().ToUpper();
                BuildSnippet("if(values.Contains("+col+")) {", 12);
                BuildSnippet("model." + column.ColumnName + " = " + ConvertType(column, col),16);
                BuildSnippet("}", 12);
                BuildSnippet("");
            }
            BuildSnippet("}",8);
            return BuildSnippet();

        }

        string ConvertType(ISchemaItem column, string col)
        {
            var convertType = string.Empty;
            switch (column.ColumnType)
            {
                case "byte":
                    convertType = "ToByte";
                    break;
                case "short":
                    convertType = "ToInt16";
                    break;
                case "int":
                    convertType = "ToInt32";
                    break;
                case "long":
                    convertType = "ToInt64";
                    break;
                case "string":
                    convertType = "ToString";
                    break;
                case "DateTime":
                    convertType = "ToDateTime";
                    break;
                case "bool":
                    convertType = "ToBoolean";
                    break;
                case "decimal":
                    convertType = "ToDecimal";
                    break;
                case "double":
                    convertType = "ToDouble";
                    break;
                case "float":
                    convertType = "ToSingle";
                    break;
                case "System.Guid":
                    convertType = "Guid.Parse( Convert.ToString(values["+ col + "]) ?? string.Empty );";
                    break;

            }

            if (column.ColumnType != "System.Guid")
            {
                var convert = "Convert." + convertType + "(values[" + col + "])";
                if (column.AllowDbNull)
                {
                    switch (column.ColumnType)
                    {
                        case "string":
                            convertType = "null;";
                            break;
                        case "short":
                            convertType = "(short) (values["+col+"] != null ? "+convert+" : ("+column.ColumnType+"?)null);";
                            break;
                        case "decimal":
                            convertType = "values[" + col + "] != null ? " + convert + " : 0M;";
                            break;
                        case "bool":
                            convertType = "(bool) (values["+col+"] != null ? "+convert+" : ("+column.ColumnType+"?)null);";
                            break;
                        default:
                            convertType = "values["+col+"] != null ? "+convert+" : ("+column.ColumnType+"?)null;";
                            break;
                    }


                    //if (column.ColumnType == "decimal")
                    //    convertType = "values[" + col + "] != null ? " + convert + " : 0M;";
                    //else
                    //    convertType = "values[" + col + "] != null ? " + convert + " : (" + column.ColumnType + "?)null;";
                }
                else
                    convertType = convert + ";";
            }
            return convertType;
        }
    }
}


