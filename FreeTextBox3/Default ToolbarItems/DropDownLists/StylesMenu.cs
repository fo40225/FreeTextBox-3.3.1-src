using System;
using System.Drawing;
using System.Collections;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarDropDownList with Paragraph JavaScript functions builtin
	/// </summary>	
	public class StylesMenu : ToolbarDropDownList {
		public StylesMenu() : base("StylesMenu","FTB_SetStyle") {
			this.isBuiltIn = true;
			this.builtInScript = @"this.ftb.SetStyle(this.list.options[this.list.options.selectedIndex].value); ";
			this.builtInStateScript = @"return this.ftb.GetStyle();";
			this.className = "StylesMenu";
		}
	}
}