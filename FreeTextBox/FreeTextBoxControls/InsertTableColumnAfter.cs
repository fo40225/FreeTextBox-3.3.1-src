using System;
namespace FreeTextBoxControls
{
	public class InsertTableColumnAfter : ToolbarButton
	{
		public InsertTableColumnAfter() : base("Insert Table Column After", "inserttablecolumnafter")
		{
			base.isProFeature = true;
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 16;
			base.builtInScript = "this.ftb.InsertTableColumnAfter();";
			base.builtInEnabledScript = "if (this.ftb.GetNearest('td')) this.disabled=false; else this.disabled=true; ";
			base.className = "InsertTableColumnAfter";
		}
	}
}
