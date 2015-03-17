using System;
namespace FreeTextBoxControls
{
	public class SymbolsMenu : ToolbarDropDownList
	{
		public SymbolsMenu() : base("Symbols Menu", "FTB_SymbolsMenu")
		{
			base.isBuiltIn = true;
			base.builtInScript = "this.ftb.InsertHtml(this.list.options[this.list.options.selectedIndex].value);";
			base.className = "SymbolsMenu";
		}
	}
}
