using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with Insert Table JavaScript functions builtin
	/// </summary>	
	public class InsertTable : ToolbarButton {
		public InsertTable() : base("Insert Table","inserttable") {
			base.isProFeature = false;
			base.isBuiltIn = true;
			this.BuiltInButtonOffset = 15;
			this.builtInScript = @"this.ftb.InsertTableWindow();";
			this.className = "InsertTable";
		}
	}
}
