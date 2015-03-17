using System;
namespace FreeTextBoxControls
{
	public class InsertTableRowBefore : ToolbarButton
	{
		public InsertTableRowBefore() : base("Insert Table Row Before", "inserttablerowbefore")
		{
			base.isProFeature = true;
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 42;
			base.builtInScript = "this.ftb.InsertTableRowBefore();";
			base.builtInEnabledScript = "if (this.ftb.GetNearest('td')) this.disabled=false; else this.disabled=true; ";
			base.className = "InsertTableRowBefore";
		}
	}
}
