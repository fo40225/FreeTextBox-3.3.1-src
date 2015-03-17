using System;
namespace FreeTextBoxControls
{
	public class InsertTextArea : ToolbarButton
	{
		public InsertTextArea() : base("Insert TextArea", "inserttextarea")
		{
			base.isProFeature = true;
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 52;
			base.builtInScript = "this.ftb.InsertTextArea();";
			base.builtInEnabledScript = "this.disabled=!this.ftb.IsInForm();";
			base.className = "InsertTextArea";
		}
	}
}
