using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with Insert InsertCheckBox JavaScript functions builtin
	/// </summary>	
	public class InsertRadioButton : ToolbarButton {
		public InsertRadioButton() : base("Insert RadioButton","insertradiobutton") {
			base.isProFeature = true;
			base.isBuiltIn = true;
			this.BuiltInButtonOffset = 51;
			this.builtInScript = @"this.ftb.InsertRadioButton();";
			this.builtInEnabledScript = @"this.disabled=!this.ftb.IsInForm();";
			this.className = "InsertRadioButton";
		}
	}
}
