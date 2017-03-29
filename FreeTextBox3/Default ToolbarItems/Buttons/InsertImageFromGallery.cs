using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with InsertImageFromGallery JavaScript functions builtin
	/// </summary>	
	public class InsertImageFromGallery : ToolbarButton {
		public InsertImageFromGallery() : base("Insert Image From Gallery","insertimagefromgallery") {
			this.isBuiltIn = true;
			this.isProFeature = false;
			this.BuiltInButtonOffset = 21;
			this.builtInScript = @"this.ftb.InsertImageFromGallery();";
			this.className = "InsertImageFromGallery";
		}
	}
}
