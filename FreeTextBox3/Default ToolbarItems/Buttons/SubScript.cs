using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with SubScript JavaScript functions builtin
	/// </summary>	
	public class SubScript : ToolbarButton {
		public SubScript() : base("SubScript","subscript") {
			isBuiltIn = true;
			this.BuiltInButtonOffset = 35;
			this.CommandIdentifier = "subscript";
			this.builtInScript = String.Empty;
			this.className = "SubScript";
		}
	}
}