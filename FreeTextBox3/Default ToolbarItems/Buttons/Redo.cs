using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with Redo JavaScript functions builtin
	/// </summary>	
	public class Redo : ToolbarButton {
		public Redo() : base("Redo","redo") {
			isBuiltIn = true;
			this.CommandIdentifier = "redo";
			this.htmlModeEnabled = false;
			this.BuiltInButtonOffset = 30;
			this.builtInScript = "this.ftb.Redo();";
			this.builtInEnabledScript = "this.disabled=!this.ftb.CanRedo();";
			this.className = "Redo";
		}
	}
}
