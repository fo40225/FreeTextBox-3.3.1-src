using System;
namespace FreeTextBoxControls
{
	public class InsertImageFromGallery : ToolbarButton
	{
		public InsertImageFromGallery() : base("Insert Image From Gallery", "insertimagefromgallery")
		{
			base.isBuiltIn = true;
			base.isProFeature = false;
			base.BuiltInButtonOffset = 21;
			base.builtInScript = "this.ftb.InsertImageFromGallery();";
			base.className = "InsertImageFromGallery";
		}
	}
}
