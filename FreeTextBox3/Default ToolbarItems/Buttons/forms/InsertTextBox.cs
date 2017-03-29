using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with Insert InsertCheckBox JavaScript functions builtin
	/// </summary>	
	public class InsertTextBox : ToolbarButton {
		public InsertTextBox() : base("Insert TextBox","inserttextbox") {
			base.isProFeature = true;
			base.isBuiltIn = true;
			this.BuiltInButtonOffset = 53;
			this.builtInScript = @"this.ftb.InsertTextBox();";
			this.builtInEnabledScript = @"this.disabled=!this.ftb.IsInForm();";
			this.className = "InsertTextBox";
		}
	}
}
