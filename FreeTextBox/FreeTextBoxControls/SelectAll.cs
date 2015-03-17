using System;
namespace FreeTextBoxControls
{
	public class SelectAll : ToolbarButton
	{
		public SelectAll() : base("Select All", "selectall")
		{
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 44;
			this.htmlModeEnabled = true;
			base.builtInScript = "this.ftb.SelectAll();";
			base.className = "SelectAll";
		}
	}
}
