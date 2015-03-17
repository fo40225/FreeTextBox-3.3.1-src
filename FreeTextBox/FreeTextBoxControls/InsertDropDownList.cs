using System;
namespace FreeTextBoxControls
{
	public class InsertDropDownList : ToolbarButton
	{
		public InsertDropDownList() : base("Insert DropDownList", "insertdropdownlist")
		{
			base.isProFeature = true;
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 49;
			base.builtInScript = "this.ftb.InsertDropDownList();";
			base.builtInEnabledScript = "this.disabled=!this.ftb.IsInForm();";
			base.className = "InsertDropDownList";
		}
	}
}
