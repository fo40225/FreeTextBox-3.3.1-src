using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with Indent JavaScript functions builtin
	/// </summary>	
	public class Indent : ToolbarButton {
		public Indent() : base("Indent","indent") {
			isBuiltIn = true;
			this.BuiltInButtonOffset = 10;
			this.CommandIdentifier = "indent";
			this.builtInScript = @"";
			this.className = "Indent";
		}
	}
}