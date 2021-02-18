using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Humanizer;
using ZeraSystems.CodeNanite.Expansion;
using ZeraSystems.CodeStencil.Contracts;

namespace ZeraSystems.DevExtremeAspCore
{
    public partial class TreeViewMenu : ExpansionBase
    {
        private List<ISchemaItem> _tables;
        private void MainFunction()
        {
            _tables = GetTables();
            Output = Populate();
        }

        private readonly int _startCol = 12;
        private string Populate()
        {
            BuildSnippet(null);
            var excludeList = GetSettingsValue("TablesToExclude", "STENCIL_CONFIG");
            if (excludeList == null) return string.Empty;

            var tablesToExclude = excludeList.Split(',').ToList<string>();

            BuildSnippet("@(Html.DevExtreme().TreeView()",_startCol);
            BuildSnippet(".Items(items => {", _startCol + 4);

            AddMenuItem("Home", "home", "Index");
            foreach (var table in _tables)
            {
                if (tablesToExclude.Contains(table.TableName)) continue;
                var pluralName = Pluralize(table.TableName, PreserveTableName());

                AddMenuItem( General.SplitCamelCase(pluralName) , "detailslayout", "/"+pluralName+"/Index");
            }
            AddMenuItem("About", "info", "/About");

            BuildSnippet("})",_startCol+4);
            BuildSnippet(".ExpandEvent(TreeViewExpandEvent.Click)", _startCol+4);
            BuildSnippet(".SelectionMode(NavSelectionMode.Single)", _startCol+4);
            BuildSnippet(".SelectedExpr("+"selected".AddQuotes()+")", _startCol+4);
            BuildSnippet(".FocusStateEnabled(false)", _startCol+4);
            BuildSnippet(".Width(250)", _startCol+4);
            BuildSnippet(".OnItemClick("+ (GetProjectName()+".onTreeViewItemClick").AddQuotes() +")", _startCol+4 );
            BuildSnippet(")", _startCol);
            return BuildSnippet();

            void AddMenuItem(string text, string icon, string url)
            {
                BuildSnippet("items.Add()",_startCol+8);
                BuildSnippet(".Text(" + text.AddQuotes() + ")", _startCol + 12);
                BuildSnippet(".Icon("+icon.AddQuotes()+")", _startCol+12);
                BuildSnippet(".Option("+"path".AddQuotes()+", GetUrl("+url.AddQuotes()+"))", _startCol+12);
                BuildSnippet(".Selected(IsCurrentUrl("+url.AddQuotes()+"));", _startCol+12);
            }

        }

    }
}


