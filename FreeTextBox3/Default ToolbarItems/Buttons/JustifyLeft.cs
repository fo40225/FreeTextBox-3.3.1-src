using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with JustifyLeft JavaScript functions builtin
	/// </summary>	
	public class JustifyLeft : ToolbarButton {
		public JustifyLeft() : base("Justify Left","justifyleft") {
			isBuiltIn = true;
			this.BuiltInButtonOffset = 22;
			this.CommandIdentifier = "justifyleft";
			this.builtInScript = String.Empty;
			this.className = "JustifyLeft";
		}
	}
}