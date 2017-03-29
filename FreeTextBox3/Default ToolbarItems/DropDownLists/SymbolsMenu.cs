using System;
using System.Drawing;
using System.Collections;
using FreeTextBoxControls.Support;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarDropDownList with a Symbols menu
	/// </summary>	
	public class SymbolsMenu : ToolbarDropDownList {
		public SymbolsMenu() : base("Symbols Menu","FTB_SymbolsMenu") {
			this.isBuiltIn = true;
			this.builtInScript = @"this.ftb.InsertHtml(this.list.options[this.list.options.selectedIndex].value);";
			this.className = "SymbolsMenu";	
		}
	}
}
