using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with Select All JavaScript functions builtin
	/// </summary>	
	public class Preview : ToolbarButton {
		public Preview() : base("Preview","preview") {
			this.isProFeature = false;
			this.isBuiltIn = true;
			this.htmlModeEnabled = true;
			this.BuiltInButtonOffset = 45;
			this.builtInScript = @"this.ftb.Preview();";
			//this.builtInScript = @"return false;";
			this.className = "Preview";
		}
	}
}
