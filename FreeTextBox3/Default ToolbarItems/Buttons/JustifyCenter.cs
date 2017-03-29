using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with JustifyCenter JavaScript functions builtin
	/// </summary>	
	public class JustifyCenter : ToolbarButton {
		public JustifyCenter() : base("Justify Center","justifycenter") {
			isBuiltIn = true;
			this.BuiltInButtonOffset = 20;
			this.CommandIdentifier = "justifycenter";
			this.builtInScript = String.Empty;
			this.className = "JustifyCenter";
		}
	}
}