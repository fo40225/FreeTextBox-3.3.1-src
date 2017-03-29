using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with InsertRule JavaScript functions builtin
	/// </summary>	
	public class InsertRule : ToolbarButton {
		public InsertRule() : base("Insert Rule","insertrule") {
			isBuiltIn = true;
			this.BuiltInButtonOffset = 14;
			this.CommandIdentifier = "inserthorizontalrule";
			this.builtInScript = String.Empty;
			this.className = "InsertRule";
		}
	}
}
