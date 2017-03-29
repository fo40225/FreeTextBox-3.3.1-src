using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with Bold JavaScript functions builtin
	/// </summary>	
	public class Bold : ToolbarButton {
		public Bold() : base("Bold","bold") {
			base.isBuiltIn = true;
			this.BuiltInButtonOffset = 0;
			this.CommandIdentifier = "bold";
			this.builtInScript = String.Empty;
			this.className = "Bold";
		}
	}
}
