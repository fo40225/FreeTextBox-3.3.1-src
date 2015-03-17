using System;
namespace FreeTextBoxControls
{
	public class FontForeColorsMenu : ToolbarDropDownList
	{
		public FontForeColorsMenu() : base("Color")
		{
			base.isBuiltIn = true;
			base.CommandIdentifier = "forecolor";
			base.builtInScript = string.Empty;
			base.className = "FontForeColorsMenu";
		}
	}
}
