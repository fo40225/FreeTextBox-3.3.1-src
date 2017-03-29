using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with Cut JavaScript functions builtin
	/// </summary>	
	public class Cut : ToolbarButton {
		public Cut() : base("Cut","cut") {
			isBuiltIn = true;
			this.CommandIdentifier = "cut";
			this.BuiltInButtonOffset = 4;
			this.htmlModeEnabled = true;
			this.builtInScript = @"this.ftb.Cut();";
			this.className = "Cut";
		}
	}
}
