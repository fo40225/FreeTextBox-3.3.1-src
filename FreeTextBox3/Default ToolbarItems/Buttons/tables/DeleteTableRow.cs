using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with Delete Table Row JavaScript functions builtin
	/// </summary>	
	public class DeleteTableRow : ToolbarButton {
		public DeleteTableRow() : base("Delete Table Row","deletetablerow") {
			base.isProFeature = true;
			base.isBuiltIn = true;
			this.BuiltInButtonOffset = 7;
			this.builtInScript = @"this.ftb.DeleteTableRow();";
			this.builtInEnabledScript = @"if (this.ftb.GetNearest('td')) this.disabled=false; else this.disabled=true; ";
			this.className = "DeleteTableRow";
		}
	}
}
