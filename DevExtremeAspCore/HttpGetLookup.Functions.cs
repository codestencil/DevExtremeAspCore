using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Humanizer;
using ZeraSystems.CodeNanite.Expansion;
using ZeraSystems.CodeStencil.Contracts;

namespace ZeraSystems.DevExtremeAspCore
{
    public partial class HttpGetLookup : ExpansionBase
    {
        private string _table;
        private void MainFunction()
        {
            var printedTables = new List<string>();

            Output = string.Empty;
            _table = GetTable(Input);
            var foreignKeys = GetForeignKeysInTable(_table).Where(t=>t.TableName==_table).ToList();
            if (!foreignKeys.Any()) return;

            foreach (var foreignKey in foreignKeys)
            {
                var table = foreignKey.RelatedTable;
                if (printedTables.Contains(table)) continue;

                Output += Lookup(table).AddCarriage();
                printedTables.Add(table);
            }
        }

        private string Lookup(string table)
        {
            var tables = Pluralize(table, PreserveTableName());
            BuildSnippet(null);
            BuildSnippet("[HttpGet]",8);
            BuildSnippet("public async Task<ActionResult> "+tables+"Lookup(DataSourceLoadOptions loadOptions) {",8);

            var tableLabel = GetTableLabel(table);
            BuildSnippet("var lookup = from i in _context."+table, 12);
            BuildSnippet("orderby i."+tableLabel, 24);
            BuildSnippet("select new {", 24);
            BuildSnippet("Text = i."+tableLabel, 28);
            BuildSnippet("};", 24);
            BuildSnippet("return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));", 12);
            BuildSnippet("}",8);
            return BuildSnippet();

        }

        string ConvertType(ISchemaItem column, string col)
        {
            var convertType = string.Empty;
            switch (column.ColumnType)
            {
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
                case "int":
                    convertType = "ToInt32";
                    break;
                case "decimal":
                    convertType = "ToDecimal";
                    break;
                case "double":
                    convertType = "ToDouble";
                    break;

            }
            var convert =  "Convert." + convertType+"(values["+col+"])";
            if (column.AllowDbNull)
            {
                switch (column.ColumnType)
                {
                    case "string":
                        convertType = "null;";
                        break;
                    case "short":
                        convertType = "(short)values["+col+"] != null ? "+convert+" : ("+column.ColumnType+"?)null;";
                        break;
                    default:
                        convertType = "values["+col+"] != null ? "+convert+" : ("+column.ColumnType+"?)null;";
                        break;
                }

            }
            else
                convertType = convert+";";

            return convertType;
        }
    }
}


