using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with Print JavaScript functions builtin
	/// </summary>	
	public class Delete : ToolbarButton {
		public Delete() : base("Delete","delete") {
			isBuiltIn = true;
			this.BuiltInButtonOffset = 5;
			this.builtInScript = @"this.ftb.DeleteContents();";
			this.className = "Delete";
		}
	}
}
