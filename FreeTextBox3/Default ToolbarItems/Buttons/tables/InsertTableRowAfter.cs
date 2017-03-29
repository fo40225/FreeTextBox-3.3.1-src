using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with Insert Table Row After JavaScript functions builtin
	/// </summary>	
	public class InsertTableRowAfter : ToolbarButton {
		public InsertTableRowAfter() : base("Insert Table Row After","inserttablerowafter") {
			base.isProFeature = true;
			base.isBuiltIn = true;
			this.BuiltInButtonOffset = 43;
			this.builtInScript = @"this.ftb.InsertTableRowAfter();";	
			this.builtInEnabledScript = @"if (this.ftb.GetNearest('td')) this.disabled=false; else this.disabled=true; ";
			this.className = "InsertTableRowAfter";
		}
	}
}
