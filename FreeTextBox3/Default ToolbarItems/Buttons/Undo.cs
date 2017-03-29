using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with Undo JavaScript functions builtin
	/// </summary>	
	public class Undo : ToolbarButton {
		public Undo() : base("Undo","undo") {
			base.isBuiltIn = true;
			this.htmlModeEnabled = false;
			this.BuiltInButtonOffset = 38;
			this.CommandIdentifier = "undo";
			this.builtInScript = "this.ftb.Undo();";
			this.builtInEnabledScript = "this.disabled=!this.ftb.CanUndo();";
			this.className = "Undo";
		}
	}
}
