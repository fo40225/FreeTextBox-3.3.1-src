using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with Insert InsertCheckBox JavaScript functions builtin
	/// </summary>	
	public class InsertTextArea : ToolbarButton {
		public InsertTextArea() : base("Insert TextArea","inserttextarea") {
			base.isProFeature = true;
			base.isBuiltIn = true;
			this.BuiltInButtonOffset = 52;
			this.builtInScript = @"this.ftb.InsertTextArea();";
			this.builtInEnabledScript = @"this.disabled=!this.ftb.IsInForm();";
			this.className = "InsertTextArea";
		}
	}
}
