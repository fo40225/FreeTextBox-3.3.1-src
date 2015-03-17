using System;
namespace FreeTextBoxControls
{
	public class FontFacesMenu : ToolbarDropDownList
	{
		public FontFacesMenu() : base("Font")
		{
			base.isBuiltIn = true;
			base.CommandIdentifier = "fontname";
			base.builtInScript = string.Empty;
			base.className = "FontFacesMenu";
		}
	}
}
