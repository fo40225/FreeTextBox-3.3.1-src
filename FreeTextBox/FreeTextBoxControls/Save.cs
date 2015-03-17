using System;
namespace FreeTextBoxControls
{
	public class Save : ToolbarButton
	{
		public Save() : base("Save", "save")
		{
			base.isBuiltIn = true;
			this.htmlModeEnabled = false;
			base.BuiltInButtonOffset = 32;
			base.builtInScript = "this.ftb.SaveButton();";
			base.className = "Save";
		}
	}
}
