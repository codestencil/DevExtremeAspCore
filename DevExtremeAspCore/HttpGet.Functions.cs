using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZeraSystems.CodeNanite.Expansion;
using ZeraSystems.CodeStencil.Contracts;

namespace ZeraSystems.DevExtremeAspCore
{
    public partial class HttpGet : ExpansionBase
    {
        private const int StartCol = 4;

        private string _table;
        private void MainFunction()
        {
            _table = GetTable(Input);
            Output = Get();
        }


        private string Get()
        {
            var paramName = Singularize(_table).ToLower()+"Id";
            var tables = Pluralize(_table).ToLower();
            BuildSnippet(null);
            BuildSnippet("[HttpGet]");
            BuildSnippet("public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions, int "+paramName+"=-1) {");
            BuildSnippet("    var "+ Pluralize(_table).ToLower()+ " = _context."+_table+".Select(i => new {");

            var columns = GetColumns(_table, false); //.Where( t=> t.IsPrimaryKey).ToList();
            var columnsCount = 0;
            foreach (var column in columns)
            {
                if (!General.DisplayColumn(column)) continue;

                columnsCount++;
                BuildSnippet("         i."+ column.ColumnName+Comma(columnsCount, columns.Count));
            }
            BuildSnippet("  });");
            BuildSnippet(" ");
            BuildSnippet("  return "+paramName+" < 0 ?");
            BuildSnippet("     Json(await DataSourceLoader.LoadAsync("+tables+", loadOptions)) :", StartCol*2);
            BuildSnippet("     Json(await DataSourceLoader.LoadAsync("+tables+".Where(x => x."+GetPrimaryKey(_table)+" == "+paramName+"), loadOptions));", StartCol*2);
            BuildSnippet("// If you work with a large amount of data, consider specifying the PaginateViaPrimaryKey and PrimaryKey properties.");
            BuildSnippet("// In this case, keys and data are loaded in separate queries. This can make the SQL execution plan more efficient.");
            BuildSnippet("// Refer to the topic https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.");
            BuildSnippet("// loadOptions.PrimaryKey = new[] { "+"Id".AddQuotes()+" };");
            BuildSnippet("// loadOptions.PaginateViaPrimaryKey = true;");
            BuildSnippet(" ");
            //BuildSnippet("  return Json(await DataSourceLoader.LoadAsync("+table+", loadOptions));");
            BuildSnippet("}");
            return BuildSnippet();
        }

        private static string Comma(int current, int count) => current < count ? "," : string.Empty;


    }
}


