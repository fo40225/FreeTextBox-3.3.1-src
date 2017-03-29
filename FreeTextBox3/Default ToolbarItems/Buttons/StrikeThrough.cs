using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with StrikeThrough JavaScript functions builtin
	/// </summary>	
	public class StrikeThrough : ToolbarButton {
		public StrikeThrough() : base("StrikeThrough","strikethrough") {
			isBuiltIn = true;
			this.BuiltInButtonOffset = 34;
			this.CommandIdentifier = "strikethrough";
			this.builtInScript = String.Empty;
			this.className = "StrikeThrough";
		}
	}
}