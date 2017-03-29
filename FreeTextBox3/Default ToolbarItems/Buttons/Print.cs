using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with Print JavaScript functions builtin
	/// </summary>	
	public class Print : ToolbarButton {
		public Print() : base("Print","print") {
			isBuiltIn = true;
			this.BuiltInButtonOffset = 29;
			this.CommandIdentifier = "print";
			this.htmlModeEnabled = true;
			this.builtInScript = @"this.ftb.Print();";
			this.className = "Print";
		}
	}
}
