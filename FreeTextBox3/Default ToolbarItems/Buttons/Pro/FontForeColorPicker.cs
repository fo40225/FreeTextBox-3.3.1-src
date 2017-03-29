using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with FontForeColorPicker JavaScript functions builtin
	/// </summary>	
	public class FontForeColorPicker : FreeTextBoxControls.ToolbarButton {
		public FontForeColorPicker() : base("Font Fore Color Picker","fontforecolorpicker") {
			base.isProFeature = true;
			base.isBuiltIn = true;
			this.BuiltInButtonOffset = 9;
			this.builtInScript = @"this.ftb.FontForeColorPicker();";
			this.className = "FontForeColorPicker";
		}
	}
}
