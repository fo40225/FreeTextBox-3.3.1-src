using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with Insert Table Row Before JavaScript functions builtin
	/// </summary>	
	public class InsertTableRowBefore : ToolbarButton {
		public InsertTableRowBefore() : base("Insert Table Row Before","inserttablerowbefore") {
			base.isProFeature = true;
			base.isBuiltIn = true;
			this.BuiltInButtonOffset = 42;
			this.builtInScript = @"this.ftb.InsertTableRowBefore();";
			this.builtInEnabledScript = @"if (this.ftb.GetNearest('td')) this.disabled=false; else this.disabled=true; ";
			this.className = "InsertTableRowBefore";
		}
	}
}
