using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with Insert Table Column Before JavaScript functions builtin
	/// </summary>	
	public class InsertTableColumnBefore : ToolbarButton {
		public InsertTableColumnBefore() : base("Insert Table Column Before","inserttablecolumnbefore") {
			base.isProFeature = true;
			base.isBuiltIn = true;
			this.BuiltInButtonOffset = 17;
			this.builtInScript = @"this.ftb.InsertTableColumnBefore();";
			this.builtInEnabledScript = @"if (this.ftb.GetNearest('td')) this.disabled=false; else this.disabled=true; ";
			this.className = "InsertTableColumnBefore";
		}
	}
}
