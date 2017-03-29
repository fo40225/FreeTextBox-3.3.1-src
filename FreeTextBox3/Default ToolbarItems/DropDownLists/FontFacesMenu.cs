using System;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarDropDownList with Font Faces JavaScript functions builtin
	/// </summary>	
	public class FontFacesMenu : ToolbarDropDownList {
		public FontFacesMenu() : base("Font") {
			this.isBuiltIn = true;
			this.CommandIdentifier = "fontname";
			this.builtInScript = String.Empty;
			this.className = "FontFacesMenu";
		}
	}
}
