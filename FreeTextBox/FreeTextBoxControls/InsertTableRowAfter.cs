using System;
namespace FreeTextBoxControls
{
	public class InsertTableRowAfter : ToolbarButton
	{
		public InsertTableRowAfter() : base("Insert Table Row After", "inserttablerowafter")
		{
			base.isProFeature = true;
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 43;
			base.builtInScript = "this.ftb.InsertTableRowAfter();";
			base.builtInEnabledScript = "if (this.ftb.GetNearest('td')) this.disabled=false; else this.disabled=true; ";
			base.className = "InsertTableRowAfter";
		}
	}
}
