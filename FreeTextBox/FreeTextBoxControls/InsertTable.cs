using System;
namespace FreeTextBoxControls
{
	public class InsertTable : ToolbarButton
	{
		public InsertTable() : base("Insert Table", "inserttable")
		{
			base.isProFeature = false;
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 15;
			base.builtInScript = "this.ftb.InsertTableWindow();";
			base.className = "InsertTable";
		}
	}
}
