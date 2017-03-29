using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with Underline JavaScript functions builtin
	/// </summary>	
	public class Underline : ToolbarButton {
		public Underline() : base("Underline","underline") {
			isBuiltIn = true;
			this.BuiltInButtonOffset = 37;
			this.CommandIdentifier = "underline";
			this.builtInScript = String.Empty;
			this.className = "Underline";
		}
	}
}