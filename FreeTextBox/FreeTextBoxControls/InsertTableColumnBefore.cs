using System;
namespace FreeTextBoxControls
{
	public class InsertTableColumnBefore : ToolbarButton
	{
		public InsertTableColumnBefore() : base("Insert Table Column Before", "inserttablecolumnbefore")
		{
			base.isProFeature = true;
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 17;
			base.builtInScript = "this.ftb.InsertTableColumnBefore();";
			base.builtInEnabledScript = "if (this.ftb.GetNearest('td')) this.disabled=false; else this.disabled=true; ";
			base.className = "InsertTableColumnBefore";
		}
	}
}
