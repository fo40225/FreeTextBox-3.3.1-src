using System;
namespace FreeTextBoxControls
{
	public class InsertDate : ToolbarButton
	{
		public InsertDate() : base("Insert Date", "insertdate")
		{
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 11;
			base.builtInScript = "\r\n\tvar d = new Date();\r\n\tthis.ftb.InsertHtml(d.toLocaleDateString());\r\n";
			base.className = "InsertDate";
		}
	}
}
