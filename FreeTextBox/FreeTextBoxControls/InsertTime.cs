using System;
namespace FreeTextBoxControls
{
	public class InsertTime : ToolbarButton
	{
		public InsertTime() : base("Insert Time", "inserttime")
		{
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 18;
			base.builtInScript = "\r\n\tvar d = new Date();\r\n\tthis.ftb.InsertHtml(d.toLocaleTimeString());\r\n";
			base.className = "InsertTime";
		}
	}
}
