using System;
namespace FreeTextBoxControls
{
	public class EditStyle : ToolbarButton
	{
		public EditStyle() : base("Edit Style", "editstyle")
		{
			base.isProFeature = true;
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = -1;
			base.builtInScript = "this.ftb.EditStyle();";
			base.className = "EditStyle";
		}
	}
}
