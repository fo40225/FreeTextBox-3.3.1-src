using System;
namespace FreeTextBoxControls
{
	public class CreateLink : ToolbarButton
	{
		public CreateLink() : base("Create Link", "createlink")
		{
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 3;
			base.CommandIdentifier = "createlink";
			base.builtInScript = "this.ftb.CreateLink();";
			base.className = "CreateLink";
		}
	}
}
