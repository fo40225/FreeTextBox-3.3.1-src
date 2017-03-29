using System;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarDropDownList with Font Sizes JavaScript functions builtin
	/// </summary>	
	public class FontSizesMenu : ToolbarDropDownList {
		public FontSizesMenu() : base("Size") {
			this.isBuiltIn = true;
			this.CommandIdentifier = "fontsize";
			this.builtInScript = String.Empty;
			this.className = "FontSizesMenu";	
		}
	}
}
