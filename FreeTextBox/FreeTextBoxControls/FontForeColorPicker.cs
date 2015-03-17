using System;
namespace FreeTextBoxControls
{
	public class FontForeColorPicker : ToolbarButton
	{
		public FontForeColorPicker() : base("Font Fore Color Picker", "fontforecolorpicker")
		{
			base.isProFeature = true;
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 9;
			base.builtInScript = "this.ftb.FontForeColorPicker();";
			base.className = "FontForeColorPicker";
		}
	}
}
