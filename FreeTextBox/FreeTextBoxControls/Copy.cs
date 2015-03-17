using System;
namespace FreeTextBoxControls
{
	public class Copy : ToolbarButton
	{
		public Copy() : base("Copy", "copy")
		{
			base.isBuiltIn = true;
			base.CommandIdentifier = "copy";
			base.BuiltInButtonOffset = 2;
			this.htmlModeEnabled = true;
			base.builtInScript = "this.ftb.Copy();";
			base.className = "Copy";
		}
	}
}
