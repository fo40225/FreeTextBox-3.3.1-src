using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with Insert CheckBox JavaScript functions builtin
	/// </summary>	
	public class InsertCheckBox : ToolbarButton {
		public InsertCheckBox() : base("Insert CheckBox","insertcheckbox") {
			base.isProFeature = true;
			base.isBuiltIn = true;
			this.BuiltInButtonOffset = 48;
			this.builtInScript = @"this.ftb.InsertCheckBox();";
			this.builtInEnabledScript = @"this.disabled=!this.ftb.IsInForm();";
			this.className = "InsertCheckBox";
		}
	}
}
