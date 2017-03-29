using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with Insert DIV JavaScript functions builtin
	/// </summary>	
	public class InsertDiv : ToolbarButton {
		public InsertDiv() : base("Insert Div","insertdiv") {
			this.isProFeature = true;
			this.isBuiltIn = true;
			this.htmlModeEnabled = false;
			this.BuiltInButtonOffset = -1;
			this.builtInScript = @"this.ftb.InsertDiv();";
			this.className = "InsertDiv";
		}
	}
}
