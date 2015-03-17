using System;
namespace FreeTextBoxControls
{
	public class FontBackColorPicker : ToolbarButton
	{
		public FontBackColorPicker() : base("Font Back Color Picker", "fontbackcolorpicker")
		{
			base.isProFeature = true;
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 8;
			base.builtInScript = "this.ftb.FontBackColorPicker();";
			base.className = "FontBackColorPicker";
		}
	}
}
