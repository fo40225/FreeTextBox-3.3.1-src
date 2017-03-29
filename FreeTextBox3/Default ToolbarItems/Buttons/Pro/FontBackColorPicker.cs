using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with FontBackColorPicker JavaScript functions builtin
	/// </summary>	
	public class FontBackColorPicker : FreeTextBoxControls.ToolbarButton {
		public FontBackColorPicker() : base("Font Back Color Picker","fontbackcolorpicker") {
			base.isProFeature = true;
			base.isBuiltIn = true;
			this.BuiltInButtonOffset = 8;
			this.builtInScript = @"this.ftb.FontBackColorPicker();";
			this.className = "FontBackColorPicker";
		}
	}
}
