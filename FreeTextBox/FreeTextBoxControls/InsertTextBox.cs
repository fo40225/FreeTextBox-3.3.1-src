using System;
namespace FreeTextBoxControls
{
	public class InsertTextBox : ToolbarButton
	{
		public InsertTextBox() : base("Insert TextBox", "inserttextbox")
		{
			base.isProFeature = true;
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 53;
			base.builtInScript = "this.ftb.InsertTextBox();";
			base.builtInEnabledScript = "this.disabled=!this.ftb.IsInForm();";
			base.className = "InsertTextBox";
		}
	}
}
