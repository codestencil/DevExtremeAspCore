using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Humanizer;
using ZeraSystems.CodeNanite.Expansion;
using ZeraSystems.CodeStencil.Contracts;

namespace ZeraSystems.DevExtremeAspCore
{
    public partial class MasterDetail : ExpansionBase
    {
        private string _table;
        private const int StartCol = 4;
        private string _controller;

        private void MainFunction()
        {
            _table = GetTable(Input);
            _controller = Pluralize(_table, PreserveTableName()).AddQuotes();

            Output = Populate();
        }

        private string Populate()
        {
            //var table = Pluralize(_table, PreserveTableName());
            BuildSnippet(null);
            BuildSnippet(".MasterDetail(md => {", StartCol);
            //BuildSnippet("{", StartCol);
            BuildSnippet("md.Enabled("+ConfigSetting("ShowMasterDetail")+");", StartCol*2);
            BuildSnippet("md.Template(@<text>", StartCol*2);
            BuildSnippet("<div>"+_table+"</div>", StartCol*5);
            BuildSnippet("@(Html.DevExtreme().DataGrid<"+GetProjectName()+".Models.EF."+_table+">()", StartCol*5); 
                BuildSnippet(".DataSource(ds => ds.Mvc()", StartCol*6);
                BuildSnippet(".Controller("+_controller+")", StartCol*7);
                BuildSnippet(".LoadAction("+"Get".AddQuotes()+")", StartCol*7);
                BuildSnippet(".LoadParams(new { "+_table.ToLower()+"Id = new JS("+ ("data."+GetPrimaryKey(_table)).AddQuotes()+") })", StartCol*7);
                BuildSnippet(")", StartCol*6);
                BuildSnippet(".RemoteOperations(true)", StartCol*6);
                BuildSnippet(".Columns(columns => {", StartCol*6);

                BuildSnippet(AddColumns(), StartCol*6);
                BuildSnippet("})", StartCol*6);
            BuildSnippet(")", StartCol*5); 
            BuildSnippet("</text>);", StartCol*5); 

            BuildSnippet("})");
            return BuildSnippet();


        }
        private string AddColumns()
        {
            var sb = new StringBuilder();
            BuildSnippet(sb, null);
            var columns = GetColumns(_table);
            foreach (var column in columns)
            {
                BuildSnippet(sb,"", StartCol*6);
                AddFor(column);
            }
            return sb.ToString();

            void AddFor(ISchemaItem col)
            {
                var addForString = "columns.AddFor(m => m."+ col.ColumnName + ").Caption(" + col.ColumnLabel.AddQuotes() + ")";
                if (!col.IsForeignKey)
                {
                    BuildSnippet(sb,addForString + ";",StartCol*7);
                    return;
                }

                //var keyString = addForString.AddCarriage();
                BuildSnippet(sb,addForString + ".Lookup(lookup => lookup", StartCol*7);
                BuildSnippet(sb,".DataSource(ds => ds.Mvc().Controller("
                                 +_controller+").LoadAction("
                                 +(Pluralize(col.RelatedTable)+"Lookup").AddQuotes()+").Key("+col.LookupColumn.AddQuotes()+"))",StartCol*7);
                BuildSnippet(sb,".ValueExpr("+col.LookupColumn.AddQuotes()+")", StartCol*8);
                BuildSnippet(sb,".DisplayExpr("+col.LookupDisplayColumn.AddQuotes()+")", StartCol*8);
                BuildSnippet(sb,");", StartCol*7);
            }

        }
        public string ConfigSetting(string configLabel)
        {
            bool.TryParse(GetSettingsValue(configLabel, "STENCIL_CONFIG"), out var result);
            return result.ToString().ToLower();
        }


    }
}


