using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with InsertTime JavaScript functions builtin
	/// </summary>	
	public class InsertTime : ToolbarButton {
		public InsertTime() : base("Insert Time","inserttime") {
			isBuiltIn = true;
			this.BuiltInButtonOffset = 18;
			this.builtInScript = @"
	var d = new Date();
	this.ftb.InsertHtml(d.toLocaleTimeString());
";
			this.className = "InsertTime";
		}
	}
}
