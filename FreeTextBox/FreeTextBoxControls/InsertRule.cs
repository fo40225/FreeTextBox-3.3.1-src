using System;
namespace FreeTextBoxControls
{
	public class InsertRule : ToolbarButton
	{
		public InsertRule() : base("Insert Rule", "insertrule")
		{
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 14;
			base.CommandIdentifier = "inserthorizontalrule";
			base.builtInScript = string.Empty;
			base.className = "InsertRule";
		}
	}
}
