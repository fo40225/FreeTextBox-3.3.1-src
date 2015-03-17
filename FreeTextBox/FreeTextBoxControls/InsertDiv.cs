using System;
namespace FreeTextBoxControls
{
	public class InsertDiv : ToolbarButton
	{
		public InsertDiv() : base("Insert Div", "insertdiv")
		{
			base.isProFeature = true;
			base.isBuiltIn = true;
			this.htmlModeEnabled = false;
			base.BuiltInButtonOffset = -1;
			base.builtInScript = "this.ftb.InsertDiv();";
			base.className = "InsertDiv";
		}
	}
}
