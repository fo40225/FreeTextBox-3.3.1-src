using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with Delete Table Column JavaScript functions builtin
	/// </summary>	
	public class DeleteTableColumn : ToolbarButton {
		public DeleteTableColumn() : base("Delete Table Column","deletetablecolumn") {
			base.isProFeature = true;
			base.isBuiltIn = true;
			this.BuiltInButtonOffset = 6;
			this.builtInScript = @"this.ftb.DeleteTableColumn();";
			this.builtInEnabledScript = @"if (this.ftb.GetNearest('td')) this.disabled=false; else this.disabled=true; ";
			this.className = "DeleteTableColumn";
		}
	}
}
