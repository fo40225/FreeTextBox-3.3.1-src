using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with Outdent JavaScript functions builtin
	/// </summary>	
	public class Outdent : ToolbarButton {
		public Outdent() : base("Outdent","outdent") {
			isBuiltIn = true;
			this.BuiltInButtonOffset = 27;
			this.CommandIdentifier = "outdent";
			this.builtInScript = String.Empty;
			this.className = "Outdent";
		}
	}
}