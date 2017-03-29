using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with Insert Table Column After JavaScript functions builtin
	/// </summary>	
	public class InsertTableColumnAfter : ToolbarButton {
		public InsertTableColumnAfter() : base("Insert Table Column After","inserttablecolumnafter") {
			base.isProFeature = true;
			base.isBuiltIn = true;
			this.BuiltInButtonOffset = 16;
			this.builtInScript = @"this.ftb.InsertTableColumnAfter();";
			this.builtInEnabledScript = @"if (this.ftb.GetNearest('td')) this.disabled=false; else this.disabled=true; ";
			this.className = "InsertTableColumnAfter";
		}
	}
}
