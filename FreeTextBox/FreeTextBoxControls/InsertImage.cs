using System;
namespace FreeTextBoxControls
{
	public class InsertImage : ToolbarButton
	{
		public InsertImage() : base("Insert Image", "insertimage")
		{
			base.isBuiltIn = true;
			base.CommandIdentifier = "insertimage";
			base.BuiltInButtonOffset = 12;
			base.builtInScript = "this.ftb.InsertImage();";
			base.className = "InsertImage";
		}
	}
}
