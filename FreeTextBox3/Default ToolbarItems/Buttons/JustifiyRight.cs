using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with JustifyRight JavaScript functions builtin
	/// </summary>	
	public class JustifyRight : ToolbarButton {
		public JustifyRight() : base("Justify Right","justifyright") {
			isBuiltIn = true;
			this.BuiltInButtonOffset = 23;
			this.CommandIdentifier = "justifyright";
			this.builtInScript = String.Empty;
			this.className = "JustifyRight";
		}
	}
}