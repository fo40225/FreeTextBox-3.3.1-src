using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with WordClean JavaScript functions builtin
	/// </summary>	
	public class WordClean : ToolbarButton {
		public WordClean() : base("Word Clean","wordclean") {
			base.isProFeature = true;
			base.isBuiltIn = true;
			this.BuiltInButtonOffset = 40;
			this.builtInScript = @"this.ftb.WordClean();";
			this.className = "WordClean";
		}
	}
}
