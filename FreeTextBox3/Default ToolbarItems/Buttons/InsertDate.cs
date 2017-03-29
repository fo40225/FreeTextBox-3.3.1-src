using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with InsertDate JavaScript functions builtin
	/// </summary>	
	public class InsertDate : ToolbarButton {
		public InsertDate() : base("Insert Date","insertdate") {
			isBuiltIn = true;
			this.BuiltInButtonOffset = 11;
			this.builtInScript = @"
	var d = new Date();
	this.ftb.InsertHtml(d.toLocaleDateString());
";
			this.className = "InsertDate";
		}
	}
}
