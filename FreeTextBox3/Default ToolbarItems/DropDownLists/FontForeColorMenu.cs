using System;
using System.Drawing;
using System.Collections;
using FreeTextBoxControls.Support;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarDropDownList with Font Fore Color JavaScript functions builtin
	/// </summary>	
	public class FontForeColorsMenu : ToolbarDropDownList {
		public FontForeColorsMenu() : base("Color") {
			this.isBuiltIn = true;
			this.CommandIdentifier = "forecolor";
			this.builtInScript = String.Empty;
			this.className = "FontForeColorsMenu";
		}
	}
}
