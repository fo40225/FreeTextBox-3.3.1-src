using System;
namespace FreeTextBoxControls
{
	public class InsertForm : ToolbarButton
	{
		public InsertForm() : base("Insert Form", "insertform")
		{
			base.isProFeature = true;
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 46;
			base.builtInScript = "this.ftb.InsertForm();";
			base.className = "InsertForm";
		}
	}
}
