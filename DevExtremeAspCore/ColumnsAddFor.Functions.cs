using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Humanizer;
using ZeraSystems.CodeNanite.Expansion;
using ZeraSystems.CodeStencil.Contracts;

namespace ZeraSystems.DevExtremeAspCore
{
    public partial class ColumnsAddFor : ExpansionBase
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

            var columns = GetColumns(_table);
            foreach (var column in columns)
            {
                var addForString = "columns.AddFor(m => m." + column.ColumnName + ").Caption("+column.ColumnLabel.AddQuotes()+")";
                if (column.IsForeignKey && !column.LookupDisplayColumn.IsBlank())
                {
                    BuildSnippet(addForString,8);
                    BuildSnippet(".Lookup(lookup => lookup",12);
                    BuildSnippet(".DataSource(ds => ds.Mvc()",16);
                    BuildSnippet(".Controller("+ (Pluralize(column.RelatedTable, PreserveTableName())).AddQuotes()+")",20);
                    BuildSnippet(".LoadAction("+"GET".AddQuotes()+")",20);
                    BuildSnippet(".Key("+column.LookupColumn.AddQuotes()+"))",20);
                    BuildSnippet(".DisplayExpr("+column.LookupDisplayColumn.AddQuotes()+")",16);
                    BuildSnippet(".ValueExpr("+column.LookupColumn.AddQuotes()+")",16);
                    BuildSnippet(");",12);

                }
                else
                    BuildSnippet(addForString + ";",8);
            }
            BuildSnippet("");
            return BuildSnippet();

            //string GetDisplayLabel(ISchemaItem column)
            //{
            //    var result = column.RelatedTable;
            //    if (!column.LookupDisplayColumn.IsBlank())
            //        result = column.LookupDisplayColumn;
            //    return result.AddQuotes();
            //}

        }


    }
}


