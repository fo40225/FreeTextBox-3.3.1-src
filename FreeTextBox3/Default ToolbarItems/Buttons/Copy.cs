using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with Copy JavaScript functions builtin
	/// </summary>	
	public class Copy : ToolbarButton {
		public Copy() : base("Copy","copy") {
			isBuiltIn = true;
			this.CommandIdentifier = "copy";
			this.BuiltInButtonOffset = 2;
			this.htmlModeEnabled = true;
			this.builtInScript = @"this.ftb.Copy();";
			this.className = "Copy";

		}
	}
}