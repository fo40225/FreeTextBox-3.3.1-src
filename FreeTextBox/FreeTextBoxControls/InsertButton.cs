using System;
namespace FreeTextBoxControls
{
	public class InsertButton : ToolbarButton
	{
		public InsertButton() : base("Insert Button", "insertbutton")
		{
			base.isProFeature = true;
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 47;
			base.builtInScript = "this.ftb.InsertButton();";
			base.builtInEnabledScript = "this.disabled=!this.ftb.IsInForm();";
			base.className = "InsertButton";
		}
	}
}
