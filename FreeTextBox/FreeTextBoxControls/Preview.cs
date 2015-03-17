using System;
namespace FreeTextBoxControls
{
	public class Preview : ToolbarButton
	{
		public Preview() : base("Preview", "preview")
		{
			base.isProFeature = false;
			base.isBuiltIn = true;
			this.htmlModeEnabled = true;
			base.BuiltInButtonOffset = 45;
			base.builtInScript = "this.ftb.Preview();";
			base.className = "Preview";
		}
	}
}
