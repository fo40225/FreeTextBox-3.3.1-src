using System;
namespace FreeTextBoxControls
{
	public class InsertCheckBox : ToolbarButton
	{
		public InsertCheckBox() : base("Insert CheckBox", "insertcheckbox")
		{
			base.isProFeature = true;
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 48;
			base.builtInScript = "this.ftb.InsertCheckBox();";
			base.builtInEnabledScript = "this.disabled=!this.ftb.IsInForm();";
			base.className = "InsertCheckBox";
		}
	}
}
