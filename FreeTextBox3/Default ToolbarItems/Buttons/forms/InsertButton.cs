using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with Insert InsertCheckBox JavaScript functions builtin
	/// </summary>	
	public class InsertButton : FreeTextBoxControls.ToolbarButton {
		public InsertButton() : base("Insert Button","insertbutton") {
			base.isProFeature = true;
			base.isBuiltIn = true;
			this.BuiltInButtonOffset = 47;
			this.builtInScript = @"this.ftb.InsertButton();";
			this.builtInEnabledScript = @"this.disabled=!this.ftb.IsInForm();";
			this.className = "InsertButton";
		}
	}
}
