using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using ZeraSystems.CodeNanite.Expansion;
using ZeraSystems.CodeStencil.Contracts;

namespace ZeraSystems.DevExtremeAspCore
{
    /// <summary>
    /// </summary>
    [Export(typeof(ICodeStencilCodeNanite))]
    [CodeStencilCodeNanite(new[]
    {
        // 0 - Publisher: This is the name of the publisher
        "ZERA Systems Inc.",                    
        // 1 - Title: This is the title of the Code Nanite
        "Generates a TreeView Menu using table names",    
        // 2 - Details: This is the description of the Code Nanite/Plugin
        "This code nanite will generate a TreeView Menu using table namesthe PopulateMode method",
        // 3 - Version Number
        "1.0",                                 
        // 4 - Label: Label of the Code Nanite
        "TreeViewMenu",                         
        // 5 - Namespace
        "ZeraSystems.DevExtremeAspCore",  
        // 6 - Release Date
        "08/01/2020",                          
        // 7 - Name to use for Expander Label
        "DX_TREEVIEW_MENU",                     
        // 8 - Indicates that the Nanite is Schema Dependent
        "1",                                   
        // 9 - RESERVED
        "",                                    
        // 10 - link to Online Help
        ""                                    
    })]
    public partial class TreeViewMenu : ExpansionBase, ICodeStencilCodeNanite
    { 
        public string Input { get; set; }
        public string Output { get; set; }
        public int Counter { get; set; }
        public List<string> OutputList { get; set; }
        public List<ISchemaItem> SchemaItem { get; set; }
        public List<IExpander> Expander { get; set; }
        public List<string> InputList { get; set; }

        public void ExecutePlugin()
        {
            Initializer(SchemaItem, Expander);
            MainFunction();
            //Output = ExpandedText.ToString();
        }

    }
}

