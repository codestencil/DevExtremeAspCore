﻿using System;
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
        "Generates the Master-Details section",    
        // 2 - Details: This is the description of the Code Nanite/Plugin
        "This code nanite will generate the Master/Details sections of a Grid",
        // 3 - Version Number
        "1.0",                                 
        // 4 - Label: Label of the Code Nanite
        "MasterDetail",                         
        // 5 - Namespace
        "ZeraSystems.DevExtremeAspCore",  
        // 6 - Release Date
        "08/29/2020",                          
        // 7 - Name to use for Expander Label
        "DX_MASTER_DETAIL",                     
        // 8 - Indicates that the Nanite is Schema Dependent
        "1",                                   
        // 9 - RESERVED
        "",                                    
        // 10 - link to Online Help
        ""                                    
    })]
    public partial class MasterDetail : ExpansionBase, ICodeStencilCodeNanite
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

