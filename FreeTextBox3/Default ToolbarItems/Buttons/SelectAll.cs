using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with Select All JavaScript functions builtin
	/// </summary>	
	public class SelectAll : ToolbarButton {
		public SelectAll() : base("Select All","selectall") {
			this.isBuiltIn = true;
			this.BuiltInButtonOffset = 44;
			this.htmlModeEnabled = true;
			this.builtInScript = @"this.ftb.SelectAll();";
			this.className = "SelectAll";
		}
	}
}
