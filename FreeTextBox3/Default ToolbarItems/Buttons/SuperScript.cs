using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with SuperScript JavaScript functions builtin
	/// </summary>	
	public class SuperScript : ToolbarButton {
		public SuperScript() : base("SuperScript","superscript") {
			isBuiltIn = true;
			this.BuiltInButtonOffset = 36;
			this.CommandIdentifier = "superscript";
			this.builtInScript = String.Empty;
			this.className = "SuperScript";
		}
	}
}