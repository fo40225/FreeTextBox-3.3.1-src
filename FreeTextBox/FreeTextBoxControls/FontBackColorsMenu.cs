using System;
namespace FreeTextBoxControls
{
	public class FontBackColorsMenu : ToolbarDropDownList
	{
		public FontBackColorsMenu() : base("Highlight")
		{
			base.isBuiltIn = true;
			base.CommandIdentifier = "backcolor";
			base.builtInScript = string.Empty;
			base.className = "FontBackColorsMenu";
		}
	}
}
