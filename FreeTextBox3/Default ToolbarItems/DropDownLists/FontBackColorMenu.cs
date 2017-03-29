using System;
using System.Drawing;
using System.Collections;
using FreeTextBoxControls.Support;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarDropDownList with Font Back Color JavaScript functions builtin
	/// </summary>	
	public class FontBackColorsMenu : ToolbarDropDownList {
		public FontBackColorsMenu() : base("Highlight") {
			this.isBuiltIn = true;
			this.CommandIdentifier = "backcolor";
			this.builtInScript = String.Empty;
			this.className = "FontBackColorsMenu";
		}
	}
}
