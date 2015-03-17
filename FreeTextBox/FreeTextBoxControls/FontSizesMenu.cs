using System;
namespace FreeTextBoxControls
{
	public class FontSizesMenu : ToolbarDropDownList
	{
		public FontSizesMenu() : base("Size")
		{
			base.isBuiltIn = true;
			base.CommandIdentifier = "fontsize";
			base.builtInScript = string.Empty;
			base.className = "FontSizesMenu";
		}
	}
}
