using System;
namespace FreeTextBoxControls
{
	public class DeleteTableRow : ToolbarButton
	{
		public DeleteTableRow() : base("Delete Table Row", "deletetablerow")
		{
			base.isProFeature = true;
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 7;
			base.builtInScript = "this.ftb.DeleteTableRow();";
			base.builtInEnabledScript = "if (this.ftb.GetNearest('td')) this.disabled=false; else this.disabled=true; ";
			base.className = "DeleteTableRow";
		}
	}
}
