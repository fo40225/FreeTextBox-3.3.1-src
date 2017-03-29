using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with Style Editing JavaScript functions builtin
	/// </summary>	
	public class EditStyle : ToolbarButton {
		public EditStyle() : base("Edit Style","editstyle") {
			base.isProFeature = true;
			base.isBuiltIn = true;
			this.BuiltInButtonOffset = -1;
			this.builtInScript = @"this.ftb.EditStyle();";
			this.className = "EditStyle";
		}
	}
}
