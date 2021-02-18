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
    public partial class GridConfig : ExpansionBase
    {
        //private List<ISchemaItem> _tables;
        private void MainFunction()
        {
            Output = Populate();
        }

        private const int _startCol = 4;

        private string Populate()
        {
            BuildSnippet(null);

            //var ShowBorders = GetSettingsValue("ShowBorders", "STENCIL_CONFIG");
            //var ShowGroupPanel = GetSettingsValue("ShowGroupPanel", "STENCIL_CONFIG");
            //var AutoExpandGroups = GetSettingsValue("AutoExpandGroups", "STENCIL_CONFIG");
            //var ShowColumnChooser = GetSettingsValue("ShowColumnChooser", "STENCIL_CONFIG");
            //var ShowRowLines = GetSettingsValue("ShowRowLines", "STENCIL_CONFIG");
            //var ShowColumnLines = GetSettingsValue("ShowColumnLines", "STENCIL_CONFIG");
            //var AllowColumnReordering = GetSettingsValue("AllowColumnReordering", "STENCIL_CONFIG");
            //var AllowColumnResizing = GetSettingsValue("AllowColumnResizing", "STENCIL_CONFIG");
            //var AlternateRows = GetSettingsValue("AlternateRows", "STENCIL_CONFIG");
            //var ShowSearchPanel = GetSettingsValue("ShowSearchPanel", "STENCIL_CONFIG");
            //var FilterRowsVisible = GetSettingsValue("FilterRowsVisible", "STENCIL_CONFIG");
            //var ShowHeaderFilter = GetSettingsValue("ShowHeaderFilter", "STENCIL_CONFIG");
            //var FocusedRowEnabled = GetSettingsValue("FocusedRowEnabled", "STENCIL_CONFIG");

            BuildSnippet(".RemoteOperations("+ConfigSetting("AllowRemoteOperations")+")", _startCol);
            BuildSnippet(".Sorting(sorting => sorting.Mode(GridSortingMode.Multiple))", _startCol);
            BuildSnippet(".Grouping(g => g.AutoExpandAll("+ConfigSetting("AutoExpandGroups")+"))", _startCol);
            BuildSnippet(".GroupPanel(gp => gp.Visible("+ConfigSetting("ShowGroupPanel")+"))", _startCol);
            BuildSnippet(".ColumnChooser(cc =>cc.Enabled("+ConfigSetting("ShowColumnChooser")+"))", _startCol);
            BuildSnippet(".AllowColumnReordering("+ConfigSetting("AllowColumnReordering")+")", _startCol);
            BuildSnippet(".RowAlternationEnabled("+ConfigSetting("AlternateRows")+")", _startCol);
            BuildSnippet(".FocusedRowEnabled("+ConfigSetting("FocusedRowEnabled")+")", _startCol);
            BuildSnippet(".ShowRowLines("+ConfigSetting("ShowRowLines")+")", _startCol);
            BuildSnippet(".FilterRow(filterRow => filterRow", _startCol);
            BuildSnippet(".Visible("+ConfigSetting("FilterRowsVisible")+")", _startCol+4);
            BuildSnippet(".ApplyFilter(GridApplyFilterMode.Auto)", _startCol+4);
            BuildSnippet(")", _startCol);
            BuildSnippet(".SearchPanel(searchPanel => searchPanel", _startCol);
            BuildSnippet(".Visible("+ConfigSetting("ShowSearchPanel")+")", _startCol+4);
            BuildSnippet(".Width(240)", _startCol+4);
            BuildSnippet(".Placeholder("+"Search...".AddQuotes()+")", _startCol+4);
            BuildSnippet(")", _startCol);
            BuildSnippet(".HeaderFilter(headerFilter => headerFilter.Visible("+ConfigSetting("ShowHeaderFilter")+"))", _startCol);
            BuildSnippet(".Editing(editing => {", _startCol);
            BuildSnippet("editing.Mode(GridEditMode.Form);", _startCol+4);
            BuildSnippet("editing.AllowUpdating("+ConfigSetting("AllowUpdating")+");", _startCol+4);
            BuildSnippet("})", 8);
            
            return BuildSnippet();
        }

        /// <summary>
        /// This will return the BOOLean configuration value of a setting as a string: "true" or "false"
        /// </summary>
        /// <param name="configLabel">Configuration Label</param>
        /// <returns>Return BOOL value as a string</returns>
        public string ConfigSetting(string configLabel)
        {
            bool.TryParse(GetSettingsValue(configLabel, "STENCIL_CONFIG"), out var result);
            return result.ToString().ToLower();
        }

    }
}


