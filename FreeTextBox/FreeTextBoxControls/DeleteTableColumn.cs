using System;
namespace FreeTextBoxControls
{
	public class DeleteTableColumn : ToolbarButton
	{
		public DeleteTableColumn() : base("Delete Table Column", "deletetablecolumn")
		{
			base.isProFeature = true;
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 6;
			base.builtInScript = "this.ftb.DeleteTableColumn();";
			base.builtInEnabledScript = "if (this.ftb.GetNearest('td')) this.disabled=false; else this.disabled=true; ";
			base.className = "DeleteTableColumn";
		}
	}
}
