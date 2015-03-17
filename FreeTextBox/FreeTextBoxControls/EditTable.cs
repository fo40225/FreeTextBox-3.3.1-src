using System;
namespace FreeTextBoxControls
{
	public class EditTable : ToolbarButton
	{
		public EditTable() : base("Edit Table", "edittable")
		{
			base.isProFeature = true;
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 15;
			base.builtInScript = "this.ftb.EditTable();";
			base.builtInEnabledScript = "if (this.ftb.GetNearest('tr')) this.disabled=false; else this.disabled=true; ";
			base.className = "EditTable";
		}
	}
}
