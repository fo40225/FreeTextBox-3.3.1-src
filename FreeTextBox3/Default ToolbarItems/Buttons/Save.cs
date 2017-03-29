using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with Save JavaScript functions builtin
	/// </summary>	
	public class Save : ToolbarButton {
		public Save() : base("Save","save") {
			base.isBuiltIn = true;
			this.htmlModeEnabled = false;
			this.BuiltInButtonOffset = 32;
			this.builtInScript = @"this.ftb.SaveButton();";
			this.className = "Save";
		}
	}
}
