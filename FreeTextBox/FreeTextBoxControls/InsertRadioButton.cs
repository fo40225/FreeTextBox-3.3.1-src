using System;
namespace FreeTextBoxControls
{
	public class InsertRadioButton : ToolbarButton
	{
		public InsertRadioButton() : base("Insert RadioButton", "insertradiobutton")
		{
			base.isProFeature = true;
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 51;
			base.builtInScript = "this.ftb.InsertRadioButton();";
			base.builtInEnabledScript = "this.disabled=!this.ftb.IsInForm();";
			base.className = "InsertRadioButton";
		}
	}
}
