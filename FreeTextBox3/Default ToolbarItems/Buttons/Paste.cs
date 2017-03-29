using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with Paste JavaScript functions builtin
	/// </summary>	
	public class Paste : ToolbarButton {
		public Paste() : base("Paste","paste") {
			isBuiltIn = true;
			this.CommandIdentifier = "paste";
			this.htmlModeEnabled = true;
			this.BuiltInButtonOffset = 28;
			this.builtInScript = @"this.ftb.Paste();";
			this.className = "Paste";
		}
	}
}