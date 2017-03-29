using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with Insert Table JavaScript functions builtin
	/// </summary>	
	public class EditTable : ToolbarButton {
		public EditTable() : base("Edit Table","edittable") {
			base.isProFeature = true;
			base.isBuiltIn = true;
			this.BuiltInButtonOffset = 15;
			this.builtInScript = @"this.ftb.EditTable();";	
			this.builtInEnabledScript = @"if (this.ftb.GetNearest('tr')) this.disabled=false; else this.disabled=true; ";
			this.className = "EditTable";
		}
	}
}
