using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with Italic JavaScript functions builtin
	/// </summary>	
	public class Italic : ToolbarButton {
		public Italic() : base("Italic","italic") {
			isBuiltIn = true;
			this.BuiltInButtonOffset = 19;
			this.CommandIdentifier = "italic";
			this.builtInScript = String.Empty;
			this.className = "Italic";
		}
	}
}