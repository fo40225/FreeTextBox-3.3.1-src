using System;
namespace FreeTextBoxControls
{
	public class WordClean : ToolbarButton
	{
		public WordClean() : base("Word Clean", "wordclean")
		{
			base.isProFeature = true;
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 40;
			base.builtInScript = "this.ftb.WordClean();";
			base.className = "WordClean";
		}
	}
}
